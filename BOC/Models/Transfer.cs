namespace BOC.Models
{
    public record Transfer
    {
        public Account DebitedAccount { get; init; }
        public Account TransferedAccount { get; init; }

        public AccountState DebitedAccountState { get; init; }
        public AccountState TransferedAccountState { get; init; }

    }
}
