namespace BOC.Core.Errors
{
    public static class Errors
    {
        public static AccountNotActive AccountNotActive => new AccountNotActive();

        public static InsufficientBalance InsufficientBalance => new InsufficientBalance();
    }
}
