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

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    // User-Address
        //    modelBuilder.Entity<Address>()
        //        .HasOne(a => a.User)
        //        .WithMany(u => u.Addresses)
        //        .HasForeignKey(a => a.UserId)
        //        .OnDelete(DeleteBehavior.Cascade);

        //    // Product → Category (Many-to-One): Each Product belongs to one Category.
        //    modelBuilder.Entity<Product>()
        //        .HasOne(p => p.Category)              // each Product has one Category
        //        .WithMany(c => c.Products)            // one Category has many Products
        //        .HasForeignKey(p => p.CategoryId)     // foreign key in Product
        //        .OnDelete(DeleteBehavior.Restrict);   // Category cannot be deleted if it has Products

        //    // User → CartItem (One-to-Many): A User can have multiple CartItems.
        //    modelBuilder.Entity<CartItem>()
        //        .HasOne(c => c.User)                  // each CartItem has one User
        //        .WithMany()                           // we are not using a navigation collection on User side
        //        .HasForeignKey(c => c.UserId)         // foreign key in CartItem
        //        .OnDelete(DeleteBehavior.Cascade);    // when User is deleted, their cart items are also deleted

        //    // Product → CartItem (One-to-Many): A Product can be added to multiple carts.
        //    modelBuilder.Entity<CartItem>()
        //        .HasOne(c => c.Product)               // each CartItem has one Product
        //        .WithMany()                           // no navigation collection in Product
        //        .HasForeignKey(c => c.ProductId)      // foreign key in CartItem
        //        .OnDelete(DeleteBehavior.Restrict);   // Product cannot be deleted if it exists in carts

        //    // User → Order (One-to-Many): A User can place multiple Orders.
        //    modelBuilder.Entity<Order>()
        //        .HasOne(o => o.User)                  // each Order has one User
        //        .WithMany(u => u.Orders)              // one User has many Orders
        //        .HasForeignKey(o => o.UserId)         // foreign key in Order
        //        .OnDelete(DeleteBehavior.Cascade);    // deleting a User deletes their Orders

        //    // Order → OrderItem (One-to-Many): Each Order can have multiple OrderItems.
        //    modelBuilder.Entity<OrderItem>()
        //        .HasOne(oi => oi.Order)               // each OrderItem belongs to one Order
        //        .WithMany(o => o.OrderItems)          // one Order has many OrderItems
        //        .HasForeignKey(oi => oi.OrderId)      // foreign key in OrderItem
        //        .OnDelete(DeleteBehavior.Cascade);    // deleting Order deletes OrderItems

        //    // Product → OrderItem (One-to-Many): A Product can be in many OrderItems.
        //    modelBuilder.Entity<OrderItem>()
        //        .HasOne(oi => oi.Product)             // each OrderItem has one Product
        //        .WithMany()                           // no navigation collection in Product
        //        .HasForeignKey(oi => oi.ProductId)    // foreign key in OrderItem
        //        .OnDelete(DeleteBehavior.Restrict);   // Product cannot be deleted if it's in any OrderItem

        //    // Order → Payment (One-to-One): One Payment per Order
        //    modelBuilder.Entity<Payment>()
        //        .HasOne(p => p.Order)                 // each Payment is for one Order
        //        .WithOne()                            // one-to-one relation, no reverse nav prop
        //        .HasForeignKey<Payment>(p => p.OrderId) // foreign key in Payment
        //        .OnDelete(DeleteBehavior.Cascade);    // deleting Order deletes its Payment

        //    // User → Review (One-to-Many): A User can leave multiple Reviews.
        //    modelBuilder.Entity<Review>()
        //        .HasOne(r => r.User)                  // each Review written by one User
        //        .WithMany(u => u.Reviews)             // one User has many Reviews
        //        .HasForeignKey(r => r.UserId)         // foreign key in Review
        //        .OnDelete(DeleteBehavior.Restrict);   // User cannot be deleted if they have Reviews

        //    // Product → Review (One-to-Many): A Product can have multiple Reviews.
        //    modelBuilder.Entity<Review>()
        //        .HasOne(r => r.Product)               // each Review is for one Product
        //        .WithMany(p => p.Reviews)             // one Product has many Reviews
        //        .HasForeignKey(r => r.ProductId)      // foreign key in Review
        //        .OnDelete(DeleteBehavior.Cascade);    // deleting Product deletes its Reviews
        //}

        //internal void Delete(object item)
        //{
        //    throw new NotImplementedException();
        //}

        //internal async Task<List<OrderItem>> GetAllAsync()
        //{
        //    throw new NotImplementedException();
        //}

        //internal async Task GetByIdAsync(int id)
        //{
        //    throw new NotImplementedException();
        //}

        //internal async Task SaveAsync()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
