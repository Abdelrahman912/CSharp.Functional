namespace BOC.Models
{
    public record MakeTransfer
    {
        public int  DebitedAccountId { get; init; }
        public int TransferedAccountId { get; init; }
        public decimal Amount { get; set; }
    }
}
