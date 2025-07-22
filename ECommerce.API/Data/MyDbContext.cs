using Microsoft.EntityFrameworkCore;
using ECommerce.API.Entities.Concrete;

namespace ECommerce.API.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<ShippingCompany> ShippingCompanies { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasConversion<string>(); // Enum'u string olarak sakla

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Address)
                .WithMany()
                .HasForeignKey(o => o.AddressId)
                .OnDelete(DeleteBehavior.Restrict); // Cascade çakışmasını önle

            modelBuilder.Entity<Order>()
                .HasOne(o => o.ShippingCompany)
                .WithMany()
                .HasForeignKey(o => o.ShippingCompanyId)
                .OnDelete(DeleteBehavior.Restrict); // Kargo firması silinirse siparişler silinmesin
        }

        

    }
}
