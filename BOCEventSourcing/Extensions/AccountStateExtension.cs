﻿using BOCEventSourcing.Domain;
using BOCEventSourcing.Enums;
using BOCEventSourcing.Events;
using CSharp.Functional.Constructs;

namespace BOCEventSourcing.Extensions
{
    public static class AccountStateExtension
    {
        public static AccountState Credit(this AccountState oldState, decimal amount) =>
            oldState with { Balance = oldState.Balance + amount };

        public static AccountState Debit(this AccountState oldState, decimal amount) =>
            oldState.Credit(-amount);

        public static AccountState Apply(this AccountState account , Event evt)=>
            new Pattern()
            {
                (DepositedAccount e) => account.Credit(e.Amount),
                (DebitedTransfer e) => account.Debit(e.DebitedAmount),
                (FrozeAccount e)=> account with {Status = AccountStatus.Frozen}

            }.Match(evt);
    }
}