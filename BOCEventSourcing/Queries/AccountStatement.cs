using BOC.Core.Domain;

namespace BOC.Core.Queries
{
    public record AccountStatement
    {
        public int Month { get; init; }
        public int Year { get; init; }
        public decimal StartingBalance { get; init; }
        public decimal EndBalance { get; init; }
        public IEnumerable<Transaction> Transactions { get; init; }
    }
}
