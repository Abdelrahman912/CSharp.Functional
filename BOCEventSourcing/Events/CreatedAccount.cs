namespace BOCEventSourcing.Events
{
    public sealed record CreatedAccount:Event
    {
        public CurrencyCode Currency { get; init; }
    }
}
