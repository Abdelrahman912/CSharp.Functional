namespace BOC.Core.Events
{
    public sealed record DepositedCash:Event
    {
        public decimal Amount { get; init; }
        public Guid BranchId { get; init; }
    }
}
