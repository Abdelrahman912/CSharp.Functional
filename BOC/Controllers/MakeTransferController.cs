using BOC.BLL;
using BOC.Models;
using CSharp.Functional.Constructs;
using CSharp.Functional.Extensions;
using Microsoft.AspNetCore.Mvc;
using static CSharp.Functional.Extensions.OptionExtension;

namespace BOC.Controllers
{
    public class MakeTransferController : ControllerBase
    {
        private readonly IValidator<MakeTransfer> _validator = new MakeTransferValidator();
        private readonly IRepository<Account> _accounts = new AccountRepository();
        private readonly IRepository<AccountState> _accountStates = new AccountStateRepository();
        private readonly ISwiftService _swiftService = new SwiftService();

        [HttpPost, Route("api/MakeTransfer")]
        public TransferReport MakeTransfer([FromBody] MakeTransfer transfer)
        {
            //if (_validator.IsValid(transfer))
            //    return Book(transfer);
            //else
            //    return null;
            var result = ((Option<MakeTransfer>)Some(transfer))
                 .Where(_validator.IsValid)
                 .Map(Book)
                 .Match(() => null, val => val);
            return result;
        }

        private TransferReport Book(MakeTransfer transfer)
        {
            var resultTrans = from debitAcc in _accounts.Get(transfer.DebitedAccountId)
                              from debositAcc in _accounts.Get(transfer.TransferedAccountId)
                              from trans in debitAcc.TransferTo(debositAcc, transfer.Amount)
                              select trans;
           var result =  resultTrans.ForEach(transfer =>
            {
                _accounts.Save(transfer.DebitedAccount);
                _accounts.Save(transfer.TransferedAccount);
                _accountStates.Save(transfer.DebitedAccountState);
                _accountStates.Save(transfer.TransferedAccountState);
            });
            return result.Match(() => new TransferReport() { Transfer = transfer, Status = "Fail" }, _ => new TransferReport() { Transfer = transfer, Status = "Success" });
        }


    }
}
