using BOC.Core.Domain;
using BOC.Core.Events;
using CSharp.Functional.Constructs;
using CSharp.Functional.Extensions;
using static CSharp.Functional.Extensions.OptionExtension;

namespace BOC.Core.Extensions
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

        public static Option<AccountState> From(this IEnumerable<Event> events) =>
            events.Match(
                Empty: () => (Option<AccountState>)None,
                Otherwise: (createdAcc, otherEvents) =>
                   Some(otherEvents.Aggregate(((CreatedAccount)createdAcc).Create(), (soFar, current) => soFar.Apply(current)))
                );



    }
}
