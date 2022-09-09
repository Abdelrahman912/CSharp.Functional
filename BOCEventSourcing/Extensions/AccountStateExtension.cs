using BOC.Core.Commands;
using BOC.Core.Domain;
using BOC.Core.Enums;
using BOC.Core.Events;
using CSharp.Functional.Constructs;
using static BOC.Core.Errors.Errors;

namespace BOC.Core.Extensions
{
    public static class AccountStateExtension
    {
        private static AccountState Credit(this AccountState oldState, decimal amount) =>
            oldState with { Balance = oldState.Balance + amount };

        private static AccountState Debit(this AccountState oldState, decimal amount) =>
            oldState.Credit(-amount);

        public static AccountState Apply(this AccountState account , Event evt)=>
            new Pattern()
            {
                (DepositedCash e) => account.Credit(e.Amount),
                (DebitedTransfer e) => account.Debit(e.DebitedAmount),
                (FrozeAccount e)=> account with {Status = AccountStatus.Frozen}

            }.Match(evt);


        public static Validation<(Event Event, AccountState NewState)> Credit(this AccountState oldSate, DepositCash cmd)
        {
            if (oldSate.Status != AccountStatus.Active)
                return AccountNotActive;

            var evt = cmd.AsEvent();
            var newState = oldSate.Apply(evt);
            return (evt, newState);
        }

        public static Validation<(Event Event , AccountState NewState)> Debit(this AccountState oldSate , MakeTransfer cmd)
        {
            if (oldSate.Status != AccountStatus.Active)
                return AccountNotActive;

            if (oldSate.Balance - cmd.Amount < oldSate.AllowedOverdraft)
                return InsufficientBalance;

            var evt = cmd.AsEvent();
            var newState = oldSate.Apply(evt);
            return (evt, newState);
        }
    }
}
