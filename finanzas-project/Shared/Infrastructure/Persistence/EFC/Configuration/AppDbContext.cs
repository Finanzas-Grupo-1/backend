using finanzas_project.IAM.Domain.Model.Aggregates;
using finanzas_project.Shared.Infrastructure.Persistence.EFC.Configuration.Extensions;
using EntityFrameworkCore.CreatedUpdatedDate.Extensions;
using Microsoft.EntityFrameworkCore;
using finanzas_project.BonusesManagement.Domain.Model.Aggregates;
using finanzas_project.BonusesManagement.Domain.Model.Entities;

namespace finanzas_project.Shared.Infrastructure.Persistence.EFC.Configuration
{
    public class AppDbContext(DbContextOptions options) : DbContext(options)
    {
        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            base.OnConfiguring(builder);
            // Enable Audit Fields Interceptors
            builder.AddCreatedUpdatedInterceptor();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);




            //BONUSES MANAGEMENT CONTEXT


            builder.Entity<Bond>().HasKey(b => b.Id);
            builder.Entity<Bond>().Property(b => b.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Entity<Bond>().Property(b => b.Name).IsRequired().HasMaxLength(200);
            builder.Entity<Bond>().Property(b => b.NominalValue).IsRequired();
            builder.Entity<Bond>().Property(b => b.CommercialValue).IsRequired();
            builder.Entity<Bond>().Property(b => b.Years).IsRequired();
            builder.Entity<Bond>().Property(b => b.PaymentsPerYear).IsRequired();
            builder.Entity<Bond>().Property(b => b.CouponRate).IsRequired();
            builder.Entity<Bond>().Property(b => b.Currency).IsRequired();
            builder.Entity<Bond>().Property(b => b.MarketRate).IsRequired();

            builder.Entity<Bond>()
             .HasMany(b => b.CashFlows)
             .WithOne(cf => cf.Bond)
             .HasForeignKey(cf => cf.BondId)
             .OnDelete(DeleteBehavior.Cascade);


            builder.Entity<CashFlow>().HasKey(cf => cf.Id);
            builder.Entity<CashFlow>().Property(b => b.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Entity<CashFlow>().Property(cf => cf.Period).IsRequired();
            builder.Entity<CashFlow>().Property(cf => cf.PaymentDate).IsRequired();
            builder.Entity<CashFlow>().Property(cf => cf.Interest).IsRequired();
            builder.Entity<CashFlow>().Property(cf => cf.Amortization).IsRequired();
            builder.Entity<CashFlow>().Property(cf => cf.RemainingDebt).IsRequired();










            // IAM Context
            builder.Entity<User>().HasKey(u => u.Id);
            builder.Entity<User>().Property(u => u.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Entity<User>().Property(u => u.Username).IsRequired();
            builder.Entity<User>().Property(u => u.PasswordHash).IsRequired();
            // Apply SnakeCase Naming Convention
            builder.UseSnakeCaseWithPluralizedTableNamingConvention();
        }
    }
}
