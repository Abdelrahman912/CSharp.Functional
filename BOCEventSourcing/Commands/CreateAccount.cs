namespace BOC.Core.Commands
{
    public record CreateAccount:Command
    {
        public CurrencyCode Currency { get; init; }
    }
}
