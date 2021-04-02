using Microsoft.EntityFrameworkCore;

namespace carlibraryapi.Model
{
    public class CarLibraryContext : DbContext
    {
        public CarLibraryContext(DbContextOptions<CarLibraryContext> options) : base(options) {}

        protected override void OnModelCreating(ModelBuilder builder){
            //builder.Entity<Brand>().HasAlternateKey(b => b.Name);
            //builder.Entity<Model>().HasAlternateKey(m => new {m.Name,m.Brand});
        }

        public DbSet<CarBrand> CarBrands {get; set;}
        public DbSet<CarModel> CarModels {get; set;}
        
    }
}