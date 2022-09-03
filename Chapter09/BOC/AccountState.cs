namespace Chapter09.BOC
{
    public sealed record AccountState
    {
        public AccountStatus Status { get; init; }
        public CurrencyCode Currency { get; init; }
        public decimal AllowedOverdraft { get; init; }
        public List<Transaction> Transactions { get; init; }


        public AccountState WithStatus(AccountStatus newStatus) =>
            new AccountState()
            {
                Status = newStatus,
                Currency = this.Currency,
                AllowedOverdraft = this.AllowedOverdraft,
                Transactions = this.Transactions
            };

                

    }
}
