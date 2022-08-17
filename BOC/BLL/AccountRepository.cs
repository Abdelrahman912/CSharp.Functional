using BOC.Context;
using BOC.Models;
using CSharp.Functional.Constructs;
using static CSharp.Functional.Extensions.OptionExtension;

namespace BOC.BLL
{
    public class AccountRepository : IRepository<Account>
    {
        private readonly BOCContext _context = new BOCContext();
        public Option<Account> Get(int id)
        {
           var account =  _context.Accounts.FirstOrDefault(acc => acc.AccountId == id);
            return account == null ? None : Some(account);
        }

        public void Save( Account t)
        {
           var account =  _context.Accounts.FirstOrDefault(acc => acc.AccountId == t.AccountId);
            account.CurrentBalance = t.CurrentBalance;
            _context.SaveChanges();
        }
    }
}
