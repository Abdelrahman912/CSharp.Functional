using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOC.Models
{
    public record Transaction
    {
        public Account Account { get; init; }
        public AccountState State { get; set; }
    }
}
