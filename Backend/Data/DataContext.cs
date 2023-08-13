using Microsoft.EntityFrameworkCore;
using Schema;

namespace Data
{
    public class DataContext : DbContext {
        private readonly IConfiguration _configuration;

        public DataContext(IConfiguration configuration){
            _configuration = configuration;
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Authentication> Authentication { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseMySQL(_configuration.GetConnectionString("Default") ?? "");
        }
    }
}