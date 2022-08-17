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
        public readonly IValidator<MakeTransfer> _validator = new MakeTransferValidator();

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
                 .Match(()=>null , val=>val);
            return result;
        }

        private TransferReport Book(MakeTransfer transfer) =>
            new TransferReport()
            {
                Transfer = transfer,
                Status = "Success"
            };

    }
}
