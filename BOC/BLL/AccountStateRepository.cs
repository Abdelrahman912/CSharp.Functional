using BOC.Context;
using BOC.Models;
using CSharp.Functional.Constructs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CSharp.Functional.Extensions.OptionExtension;

namespace BOC.BLL
{
    public class AccountStateRepository : IRepository<AccountState>
    {
        private readonly BOCContext _context = new BOCContext();

        public Option<AccountState> Get(int id)
        {
            var account = _context.AccountStates.FirstOrDefault(acc => acc.AccountStateId == id);
            return account == null ? None : Some(account);
        }

        public void Save( AccountState t)
        {
            _context.AccountStates.Add(t);
            _context.SaveChanges();
        }
    }
}
