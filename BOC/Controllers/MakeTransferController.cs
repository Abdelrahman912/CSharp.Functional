﻿using BOC.Core.Commands;
using BOC.Core.Domain;
using BOC.Core.Events;
using BOC.Core.Extensions;
using BOC.Dtos;
using CSharp.Functional.Constructs;
using Microsoft.AspNetCore.Mvc;
using CSharp.Functional.Extensions;

namespace BOC.Controllers
{
    public class MakeTransferController : ControllerBase
    {

        private readonly Func<MakeTransfer, Validation<MakeTransfer>> _validate;
        private readonly Func<Guid, AccountState> _getAccount;
        private readonly Action<Event> _saveAndPublish;


        public MakeTransferController(Func<Guid,AccountState> getAccount ,
                                      Action<Event> saveAndPublish,
                                      Func<MakeTransfer,Validation<MakeTransfer>> validate)
        {
            _getAccount = getAccount;
            _saveAndPublish = saveAndPublish;
            _validate = validate;
        }


        [HttpPost, Route("api/MakeTransfer")]
        public ResultDto<AccountState> MakeTransfer([FromBody] MakeTransfer cmd) =>
            _validate(cmd)
                .Bind(t => _getAccount(t.DebitedAccountId).Debit(t))
                .Do(tuple => _saveAndPublish(tuple.Event))
                .Match<ResultDto<AccountState>>(
                   invalid: (errs) => errs.ToList(),
                   valid: (tuple)=> tuple.NewState
                );

    }
}
