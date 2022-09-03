namespace BOCEventSourcing.Events
{
    public abstract record Event
    {
        public Guid Id { get; init; }
        public DateTime Timestamp { get; init; }
    }
}
