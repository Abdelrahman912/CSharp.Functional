using BOC.Core.Commands;
using BOC.Core.Domain;
using BOC.Core.Events;

namespace BOC.Core.Extensions
{
    public static class CommandExtension
    {
        public static DebitedTransfer AsEvent(this MakeTransfer cmd) =>
            new DebitedTransfer()
            {
                Beneficiary = cmd.Beneficiary,
                Bic = cmd.Bic,
                DebitedAmount = cmd.Amount,
                EntityId = cmd.DebitedAccountId,
                Iban = cmd.Iban,
                Reference = cmd.Reference,
                Timestamp = cmd.TimeStamp
            };

        public static DepositedCash AsEvent(this DepositCash cmd) =>
            new DepositedCash()
            {
                EntityId = cmd.DepositedAccountId,
                Amount = cmd.Amount,
                BranchId = cmd.BranchId,
                Timestamp = cmd.TimeStamp
            };

        public static CreatedAccount AsEvent(this CreateAccount cmd) =>
            new CreatedAccount()
            {
                Currency = cmd.Currency,
                EntityId = Guid.NewGuid(),
                Timestamp = DateTime.UtcNow
            };

        public static(Event Event , AccountState NewState) CreateAccount(this CreateAccount cmd)
        {
            var evt = cmd.AsEvent();
            var state = evt.Create();
            return (evt, state);
        }
    }
}
