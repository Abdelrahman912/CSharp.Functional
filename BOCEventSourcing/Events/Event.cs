namespace BOC.Core.Events
{
    public abstract record Event
    {
        public Guid EntityId { get; init; }
        public DateTime Timestamp { get; init; }
    }
}
