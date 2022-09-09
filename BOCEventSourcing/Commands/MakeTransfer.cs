namespace BOC.Core.Commands
{
    public record MakeTransfer:Command
    {
        public Guid DebitedAccountId { get; init; }

        public string Beneficiary { get; init; }
        public string Iban { get; init; }
        public string Bic { get; init; }

        public DateTime Date { get; init; }
        public decimal Amount { get; init; }
        public string Reference { get; init; }
    }
}
