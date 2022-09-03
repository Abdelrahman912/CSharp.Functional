using BOCEventSourcing.Domain;
using BOCEventSourcing.Events;

namespace BOCEventSourcing.Extensions
{
    public static class EventExtension
    {
        public static AccountState Create(this CreatedAccount evt) =>
            new AccountState()
            {
                Currency = evt.Currency,
                Status = Enums.AccountStatus.Active,
                Balance = 0
            };
    }
}
