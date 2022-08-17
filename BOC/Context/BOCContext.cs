using BOC.Models;
using Microsoft.EntityFrameworkCore;

namespace BOC.Context
{
    public class BOCContext:DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
            optionsBuilder.UseSqlServer("Data Source=.; Initial Catalog=BOC; Integrated Security = true");

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<AccountState> AccountStates { get; set; }
    }
}
