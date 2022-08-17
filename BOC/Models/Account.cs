namespace BOC.Models
{
    public record Account
    {
        public int AccountId { get; init; }
        public decimal CurrentBalance { get; init; }
        public virtual ICollection<AccountState> States { get; init; } = new HashSet<AccountState>();
    }
}
