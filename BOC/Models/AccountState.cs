namespace BOC.Models
{
    public enum State
    {
        TRANSFER,RECIEVE
    }

    public record AccountState
    {
        public int AccountStateId { get; init; }
        public State Process { get; set; }
        public decimal NewBalance { get; init; }
        public decimal ProcessAmount { get; init; }
        public decimal OldBalance { get; init; }
        public int AccountId { get; init; }
        public Account Account { get; init; }
    }
}
