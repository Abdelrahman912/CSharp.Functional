using BOC.Models;
using CSharp.Functional.Constructs;
using static CSharp.Functional.Extensions.OptionExtension;

namespace BOC.BLL
{
    public static class AccountExtension
    {
        public static Option<Transaction> Debit(this Account acc, decimal amount)
        {
            if (acc.CurrentBalance > amount)
            {
                var oldBalance = acc.CurrentBalance;
                var newBalance = acc.CurrentBalance - amount;
                var newAcc = acc with { CurrentBalance = newBalance };
                return new Transaction()
                {
                    Account = newAcc,
                    State = new AccountState()
                    {
                        OldBalance = oldBalance,
                        NewBalance = newBalance,
                        Process = State.TRANSFER,
                        AccountId = newAcc.AccountId,
                        ProcessAmount = amount
                    }
                };
            }
            else
            {
                return None;
            }
        }

        public static Transaction Deposit(this Account acc, decimal amount)
        {
            var oldBalance = acc.CurrentBalance;
            var newBalance = acc.CurrentBalance + amount;
            var newAcc = acc with { CurrentBalance = newBalance };
            return new Transaction()
            {
                Account = newAcc,
                State = new AccountState()
                {
                    OldBalance = oldBalance,
                    NewBalance = newBalance,
                    Process = State.RECIEVE,
                    AccountId = newAcc.AccountId,
                    ProcessAmount= amount
                }
            };
        }


        public static Option<Transfer> TransferTo(this Account debit, Account deposit, decimal amount)
        {
          return debit.Debit(amount)
                .Map(debitTrans =>Tuple.Create(debitTrans, deposit.Deposit(amount)))
                .Map(tuple => new Transfer()
                {
                    DebitedAccount = tuple.Item1.Account,
                    DebitedAccountState = tuple.Item1.State,
                    TransferedAccount = tuple.Item2.Account,
                    TransferedAccountState = tuple.Item2.State
                });;;
        }

    }
}
