using BOC.Core.Events;
using Microsoft.EntityFrameworkCore;

namespace BOC.Core.Data.SqlEventStore
{
    public class BOCContext:DbContext
    {
        public virtual DbSet<Event> Events { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
            optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=BOCES; Integrated Security=true;TrustServerCertificate=True; ");
       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CreatedAccount>();
            modelBuilder.Entity<DebitedTransfer>();
            modelBuilder.Entity<DepositedCash>();
            modelBuilder.Entity<FrozeAccount>();
        }
    }
}
