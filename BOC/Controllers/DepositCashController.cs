using BOC.Core.Commands;
using BOC.Core.Domain;
using BOC.Core.Events;
using BOC.Core.Extensions;
using BOC.Dtos;
using CSharp.Functional.Constructs;
using CSharp.Functional.Extensions;
using Microsoft.AspNetCore.Mvc;


namespace BOC.Controllers
{
    public class DepositCashController:ControllerBase
    {
        private readonly Func<DepositCash, Validation<DepositCash>> _validate;
        private readonly Func<Guid, AccountState> _getAccount;
        private readonly Action<Event> _saveAndPublish;


        public DepositCashController(Func<Guid, AccountState> getAccount,
                                      Action<Event> saveAndPublish,
                                      Func<DepositCash, Validation<DepositCash>> validate)
        {
            _getAccount = getAccount;
            _saveAndPublish = saveAndPublish;
            _validate = validate;
        }


        [HttpPost, Route("api/DepositCash")]
        public ResultDto<AccountState> MakeTransfer([FromBody] DepositCash cmd) =>
            _validate(cmd)
                .Bind(t => _getAccount(t.DepositedAccountId).Credit(t))
                .Do(tuple => _saveAndPublish(tuple.Event))
                .Match<ResultDto<AccountState>>(
                   invalid: (errs) => errs.ToList(),
                   valid: (tuple) => tuple.NewState
                );
    }
}
