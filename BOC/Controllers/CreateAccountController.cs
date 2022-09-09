using BOC.Core.Commands;
using BOC.Core.Domain;
using BOC.Core.Events;
using BOC.Core.Extensions;
using BOC.Dtos;
using CSharp.Functional.Constructs;
using Microsoft.AspNetCore.Mvc;
using CSharp.Functional.Extensions;

namespace BOC.Controllers
{
    public class CreateAccountController:ControllerBase
    {
        private readonly Func<CreateAccount, Validation<CreateAccount>> _validate;
        private readonly Action<Event> _saveAndPublish;


        public CreateAccountController( Action<Event> saveAndPublish,
                                      Func<CreateAccount, Validation<CreateAccount>> validate)
                                     
        {
            _saveAndPublish = saveAndPublish;
            _validate = validate;
        }


        public void test()
        {
        }

        [HttpPost, Route("api/CreateAccount")]
        public ResultDto<AccountState> MakeTransfer([FromBody] CreateAccount cmd) =>
            _validate(cmd)
                .Map(t => t.CreateAccount())
                .Do(tuple => _saveAndPublish(tuple.Event))
                .Match<ResultDto<AccountState>>(
                   invalid: (errs) => errs.ToList(),
                   valid: (tuple) => tuple.NewState
                );
    }
}
