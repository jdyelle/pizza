using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace AspirationalPizza.Library.Services.FoodItems.Repositories
{
    public class FoodItemDbContext : DbContext
    {
        public FoodItemDbContext(DbContextOptions<FoodItemDbContext> options) : base(options) { }
        public DbSet<FoodItemModel> FoodItems { get; set; }

        //Since EF Core doesn't support primitive collections, we have to convert the lists to comma separated strings
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FoodItemModel>()
                .Property(f => f.LeftToppings)
                .HasConversion(
                    v => string.Join(',', v.ToArray()),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList());

            modelBuilder.Entity<FoodItemModel>()
                .Property(f => f.RightToppings)
                .HasConversion(
                    v => string.Join(',', v.ToArray()),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList());
        }
    }
}
