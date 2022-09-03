using BOCEventSourcing.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOCEventSourcing.Domain
{
    public sealed record AccountState
    {
        public AccountStatus Status { get; init; }
        public CurrencyCode Currency { get; init; }
        public decimal Balance { get; init; }
        public decimal AllowedOverdraft { get; init; }
    }
}
