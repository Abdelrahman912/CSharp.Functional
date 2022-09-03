namespace BOC.Core.Events
{
    public sealed record DepositedAccount:Event
    {
        public decimal Amount { get; init; }
        public Guid BranchId { get; init; }
    }
}
