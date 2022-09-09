namespace BOC.Core.Commands
{
    public record DepositCash:Command
    {
        public Guid DepositedAccountId { get; init; }
        public decimal Amount { get; init; }
        public Guid BranchId { get; init; }
    }
}
