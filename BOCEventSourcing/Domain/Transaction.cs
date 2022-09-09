namespace BOC.Core.Domain
{
    public record Transaction
    {
        public DateTime Date { get; init; }
        public decimal DebitedAmount { get; init; }
        public decimal CreditedAmount { get; init; }
        public string Description { get; init; }
    }
}
