using BOC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOC.BLL
{
    public class MakeTransferValidator : IValidator<MakeTransfer>
    {
        public bool IsValid(MakeTransfer value)
        {
            return true;
        }
    }
}
