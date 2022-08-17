namespace BOC.Models
{
    public record Account
    {
        public int AccountId { get; set; }
        public decimal CurrentBalance { get; set; }
        public virtual ICollection<AccountState> States { get; set; } = new HashSet<AccountState>();
    }
}
