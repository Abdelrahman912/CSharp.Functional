using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chapter09.BOC
{
    public record Transaction
    {
        public decimal Amount { get; init; }
        public string Description { get; init; }
        public DateTime Date { get; init; }
    }
}
