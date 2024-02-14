using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace AspirationalPizza.Library.Services.Customers.Repositories
{
    public class CustomerDbContext : DbContext
    {
        public CustomerDbContext(DbContextOptions<CustomerDbContext> options) : base(options) { }
        public DbSet<CustomerModel> Customers { get; set; }
        public DbSet<CustomerAddress> CustomerAddresses { get; set; }
        public DbSet<String> Emails { get; set; }
        public DbSet<String> PhoneNumbers { get; set; }
        public DbSet<String> FavoriteFoodItems { get; set; }

        //Since EF Core doesn't support primitive collections, we have to convert the lists to comma separated strings
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CustomerModel>()
                .Property(c => c.Emails)
                .HasConversion(
                    v => string.Join(',', v.ToArray()),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList());
            modelBuilder.Entity<CustomerModel>()
                .Property(c => c.PhoneNumbers)
                .HasConversion(
                    v => string.Join(',', v.ToArray()),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList());
            modelBuilder.Entity<CustomerModel>()
                .Property(c => c.FavoriteFoodItems)
                .HasConversion(
                    v => string.Join(',', v.ToArray()),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList());
        }
    }
}
