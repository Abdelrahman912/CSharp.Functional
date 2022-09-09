using BOC.Core.Domain;
using BOC.Core.Events;
using BOC.Core.Queries;
using CSharp.Functional.Constructs;
using CSharp.Functional.Extensions;
using System.Runtime.CompilerServices;
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
                Balance = 0,
                AllowedOverdraft = 5000,
            };

        public static Option<AccountState> From(this IEnumerable<Event> events) =>
            events.Match(
                Empty: () => (Option<AccountState>)None,
                Otherwise: (createdAcc, otherEvents) =>
                   Some(otherEvents.Aggregate(((CreatedAccount)createdAcc).Create(), (soFar, current) => soFar.Apply(current)))
                );


        public static AccountStatement AsStatement(this IEnumerable<Event> events, int month, int year)
        {
            var startOfPeriod = new DateTime(year, month, 1);
            var endOfPeriod = startOfPeriod.AddMonths(1);

            var eventsBeforePeriod = events.TakeWhile(ev => ev.Timestamp < startOfPeriod);
            var eventsInPeriod = events.SkipWhile(e => e.Timestamp < startOfPeriod)
                                       .TakeWhile(ev => ev.Timestamp < endOfPeriod);
            var startingBalance = eventsBeforePeriod.Aggregate(0m, (soFar, current) => current.BalanceReducer(soFar));
            var endBalance = eventsInPeriod.Aggregate(startingBalance, (soFar, current) => current.BalanceReducer(soFar));
            var trans = eventsInPeriod.Bind(e => e.CreateTransaction());
            return new AccountStatement()
            {
                StartingBalance = startingBalance,
                EndBalance = endBalance,
                Year = year,
                Month = month,
                Transactions =trans
            };
        }

        private static decimal BalanceReducer(this Event evt, decimal bal) =>
            new Pattern()
            {
                (DepositedCash e)=> bal +e.Amount,
                (DebitedTransfer e)=>bal - e.DebitedAmount
            }
            .Default(bal)
            .Match(evt);

        public static Transaction AsTransaction(this DebitedTransfer e)
        {
            return new Transaction()
            {
                DebitedAmount = e.DebitedAmount,
                Description = $"Transfer to {e.Bic}/{e.Iban}; Ref: {e.Reference}",
                Date = e.Timestamp.Date
            };
        }

        public static Transaction AsTransaction(this DepositedCash e)
        {
            return new Transaction()
            {
                CreditedAmount = e.Amount,
                Description = $"Deposit at {e.BranchId}",
                Date = e.Timestamp.Date
            };
        }


        public static Option<Transaction> CreateTransaction(this Event evt) =>
            new Pattern()
            {
                (DepositedCash e)=> e.AsTransaction(),
                (DebitedTransfer e)=>e.AsTransaction()
            }
            .Default(None)
            .Match(evt);
    }
}
