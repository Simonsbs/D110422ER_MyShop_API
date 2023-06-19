using Microsoft.EntityFrameworkCore;
using MyShopAPI.Models;

namespace MyShopAPI.Contexts
{
    /// <summary>
    /// 
    /// </summary>
    public class MyShopContext : DbContext {
        public MyShopContext(DbContextOptions<MyShopContext> options)
            : base(options) {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        //    optionsBuilder.UseSqlServer("Server=SIMONSPC\\SQLEXPRESS02;Database=MyShop;" +
        //                                "Trusted_Connection=True;TrustServerCertificate=True;");
        //}
    }
}
