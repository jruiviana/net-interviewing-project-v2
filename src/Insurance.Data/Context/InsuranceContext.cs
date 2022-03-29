using Insurance.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Insurance.Data.Context
{
    public sealed class InsuranceContext : DbContext
    {

        public DbSet<SurchargeRate> SurchargeRates { get; set; }
        

        public InsuranceContext(DbContextOptions<InsuranceContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(InsuranceContext).Assembly);
        }
    }
}
