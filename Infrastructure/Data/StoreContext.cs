
using System.Reflection;
using core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class StoreContext :DbContext
    {
        public StoreContext(DbContextOptions options) : base(options)
        {
            
        }
        public DbSet<Product> Products{get;set;}
        public DbSet<ProductBrand> ProductBrands {get; set;}
        public DbSet<ProductType> ProductTypes {get; set;}

        //the below method will override the configuration and configuration will be made according to Productconfiguration rules.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}