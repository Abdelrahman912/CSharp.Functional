using BOC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOC.BLL
{
    public interface ISwiftService
    {
        void Wire(MakeTransfer transfer, AccountState account);
    }
}
