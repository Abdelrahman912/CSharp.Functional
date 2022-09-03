namespace BOC.Core.Events
{
    public sealed record CreatedAccount:Event
    {
        public CurrencyCode Currency { get; init; }
    }
}
