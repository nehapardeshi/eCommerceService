using eCommerceService.Entities;
using eCommerceService.Entities.Enums;
using Microsoft.EntityFrameworkCore;

namespace eCommerceService.Database
{
    public class ECommerceDbContext : DbContext
    {
        public ECommerceDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }
        //public DbSet<Address> Address { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().Ignore(c => c.OnHandQuantity);
            modelBuilder.Entity<OrderItem>().HasOne(item => item.Order).WithMany(order => order.OrderItems)
                .HasForeignKey(p => p.OrderId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Product>().HasData(new Product
            {
                Id = 1,
                Name = "Oriental Bronze Chickend",
                Description = "The Football Is Good For Training And Recreational Purposes",
                ProductCategory = (ProductCategory)1,
                IsActive = true,
                CreatedDate = DateTime.Now,
                UnitPrice = 500,
                SKU = "SK001",
                AvailableQuantity = 10,
                ReservedQuantity = 0
            },
            new Product
            {
                Id = 2,
                Name = "Gorgeous Plastic Sausages",
                Description = "Andy shoes are designed to keeping in mind durability as well as trends",
                ProductCategory = (ProductCategory)2,
                IsActive = true,
                CreatedDate = DateTime.Now,
                UnitPrice = 427,
                SKU = "SK002",
                AvailableQuantity = 20,
                ReservedQuantity = 0
            },
            new Product
            {
                Id = 3,
                Name = "Intelligent Rubber Pizza",
                Description = "Ergonomic executive chair upholstered in bonded black leather and PVC padded seat and back for all-day comfort and support",
                ProductCategory = (ProductCategory)3,
                IsActive = true,
                CreatedDate = DateTime.Now,
                UnitPrice = 200,
                SKU = "SK003",
                AvailableQuantity = 50,
                ReservedQuantity = 0
            },
            new Product
            {
                Id = 4,
                Name = "Luxurious Cotton Gloves",
                Description = "The Nagasaki Lander is the trademarked name of several series of Nagasaki sport bikes",
                ProductCategory = (ProductCategory)2,
                IsActive = true,
                CreatedDate = DateTime.Now,
                UnitPrice = 250,
                SKU = "SK004",
                AvailableQuantity = 100,
                ReservedQuantity = 0
            },
            new Product
            {
                Id = 5,
                Name = "Ergonomic Soft Tuna",
                Description = "The slim & simple Maple Gaming Keyboard from Dev Byte comes with a sleek body",
                ProductCategory = (ProductCategory)3,
                IsActive = true,
                CreatedDate = DateTime.Now,
                UnitPrice = 1000,
                SKU = "SK005",
                AvailableQuantity = 28,
                ReservedQuantity = 0
            });
            modelBuilder.Entity<Customer>().HasData(new Customer
            {
                Id = 1,
                FirstName = "John",
                LastName = "Abraham",
                Email = "john@hotmail.com",
                Phone = "12345678",
                IsActive = true,
                CreatedDate = DateTime.Now,
                StreetAddress = "Havreveien 50",
                PostalCode = "0001",
                City = "Stockholm",
                Country = "Sweden"
            },
            new Customer
            {
                Id = 2,
                FirstName = "Natasha",
                LastName = "Kousar",
                Email = "natasha@gmail.com",
                Phone = "12345678",
                IsActive = true,
                CreatedDate = DateTime.Now,
                StreetAddress = "Høvikveien 40",
                PostalCode = "0900",
                City = "Oslo",
                Country = "Norway"
            },
            new Customer
            {
                Id = 3,
                FirstName = "Marion",
                LastName = "Klintzing",
                Email = "marion@yahoomail.com",
                Phone = "12345678",
                IsActive = true,
                CreatedDate = DateTime.Now,
                StreetAddress = "Mortensrudveien 70",
                PostalCode = "0002",
                City = "Berlin",
                Country = "Germany"
            });
        }
    }
}
