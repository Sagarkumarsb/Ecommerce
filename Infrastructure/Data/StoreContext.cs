
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

            //Below code is added for handling the double data type
            if(Database.ProviderName =="Microsoft.EntityFrameworkCore.Sqlite"){
                foreach(var entityType in modelBuilder.Model.GetEntityTypes()){
                    var properties = entityType.ClrType.GetProperties().Where(p => p.PropertyType == typeof(decimal));

                    foreach(var property in properties){
                                modelBuilder.Entity(entityType.Name).Property(property.Name)
                                .HasConversion<double>();
                    }
                }
            }
        }
    }
}