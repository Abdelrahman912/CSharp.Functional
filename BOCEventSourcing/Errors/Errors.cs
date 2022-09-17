namespace BOC.Core.Errors
{
    public static class Errors
    {
        public static AccountNotActive AccountNotActive => new AccountNotActive();

        public static InsufficientBalance InsufficientBalance => new InsufficientBalance();

        public static UnknownAccountId UnknownAccountId(Guid id) =>
            new UnknownAccountId(id);

        public static ExceptionError UnExpectedError(Exception ex) =>
            new ExceptionError(ex);

    }
}
