using BOC.Core.Commands;
using BOC.Core.Domain;
using BOC.Core.Events;
using BOC.Core.Extensions;
using BOC.Dtos;
using CSharp.Functional.Constructs;
using Microsoft.AspNetCore.Mvc;
using BOC.Core.Errors;
using CSharp.Functional;
using Unit = System.ValueTuple;
using static CSharp.Functional.Functional;
using Microsoft.EntityFrameworkCore.Update.Internal;
using CSharp.Functional.Extensions;
using static CSharp.Functional.Extensions.ValidationExtension;
using CSharp.Functional.Errors;

namespace BOC.Controllers
{
    public class MakeTransferController : ControllerBase
    {

        private readonly Func<MakeTransfer, Validation<MakeTransfer>> _validate;
        private readonly Func<Guid, Task<Option<AccountState>>> _getAccount;
        private readonly Func<Event, Task> _saveAndPublish;


        public MakeTransferController(Func<Guid, Task<Option<AccountState>>> getAccount,
                                      Func<Event, Task> saveAndPublish,
                                      Func<MakeTransfer, Validation<MakeTransfer>> validate)
        {
            _getAccount = getAccount;
            _saveAndPublish = saveAndPublish;
            _validate = validate;
        }


        Func<Guid, Task<Validation<AccountState>>> GetAccount => id =>
            _getAccount(id).Map(op => op.ToValidation(() => Errors.UnknownAccountId(id)));


        Func<Event, Task<Unit>> SaveAndPublish =>
            async e =>
            {
                await _saveAndPublish(e);
                return Unit();
            };

        //[HttpPost, Route("api/MakeTransfer")]
        //public ResultDto<AccountState> MakeTransfer([FromBody] MakeTransfer cmd) =>
        //    _validate(cmd)
        //        .Bind(t => _getAccount(t.DebitedAccountId).Debit(t))
        //        .Do(tuple => _saveAndPublish(tuple.Event))
        //        .Match<ResultDto<AccountState>>(
        //           invalid: (errs) => errs.ToList(),
        //           valid: (tuple)=> tuple.NewState
        //        );


        [HttpPost, Route("api/MakeTransfer")]
        public Task<ResultDto<AccountState>> MakeTransfer([FromBody] MakeTransfer command)
        {
            var outcome = from cmd in (_validate(command).Async())
                          from acc in GetAccount(cmd.DebitedAccountId)
                          from result in acc.Debit(cmd).Async()
                          from _ in SaveAndPublish(result.Event).Map(Valid)
                          select result.NewState;
            var finalResult = outcome.Map(
                      faulted: (ex) => new ResultDto<AccountState>(new List<Error>() { Errors.UnExpectedError(ex) }),
                      completed: val => val.Match<ResultDto<AccountState>>(errs => errs.ToList(), state => state));
            return finalResult;
        }


    }
}
