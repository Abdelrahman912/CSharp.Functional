namespace BOC.Core.Events
{
    public abstract record Event
    {
        public int Id { get; init; }
        public Guid EntityId { get; init; }
        public DateTime Timestamp { get; init; }
    }
}
