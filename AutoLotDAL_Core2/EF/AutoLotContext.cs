using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using AutolotDAL_Core2.Models;

namespace AutolotDAL_Core2.EF
{
    public class AutoLotContext : DbContext
    {
        public AutoLotContext(DbContextOptions options) : base(options)
        {
        }

        internal AutoLotContext()
        {
        }

        public DbSet<AutolotDAL_Core2.Models.CreditRisk> CreditRisks { get; set; }
        public DbSet<AutolotDAL_Core2.Models.Customer> Customers { get; set; }
        public DbSet<AutolotDAL_Core2.Models.inventory> Cars { get; set; }
        public DbSet<AutolotDAL_Core2.Models.Order> Orders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = @"server=(LocalDb)\MSSQLLocalDB;database=AutoLotCore2;integrated security=True; MultipleActiveResultSets=True;App=EntityFramework;";
                optionsBuilder.UseSqlServer(connectionString, options => options.EnableRetryOnFailure())
                    .ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //create the multi column index
            modelBuilder.Entity<CreditRisk>(entity =>
            {
                entity.HasIndex(e => new { e.FirstName, e.LastName }).IsUnique();

            });
            //set the cascade options on the relationship
            modelBuilder.Entity<Order>()
                .HasOne(e => e.Car)
                .WithMany(e => e.Orders)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }

        public String GetTableName(Type type)
        {
            return Model.FindEntityType(type).GetTableName();
        }
    }
}