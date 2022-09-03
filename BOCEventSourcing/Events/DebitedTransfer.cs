namespace BOC.Core.Events
{
    public sealed record DebitedTransfer:Event
    {
        public string Beneficiary { get; init; }
        public string Iban { get; init; }
        public string Bic { get; init; }
        public decimal DebitedAmount { get; init; }
        public string Reference { get; set; }
    }
}
