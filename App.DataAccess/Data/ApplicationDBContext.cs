using App.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace App.DataAccess.Data
{
    public class ApplicationDBContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
            
        }

        public DbSet<Category>  Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<OrderHeader> OrderHeaders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // keys of Identity Table are mapped into OnModelCreating. When IdentityDBContext is added, this should be implemeneted.

            modelBuilder.Entity<Category>().HasData(
                new Category { Category_Id = 1, Name = "Action", DisplayOrder = 1 },
                new Category { Category_Id = 2, Name = "SciFi", DisplayOrder = 2},
                new Category { Category_Id = 3, Name = "History", DisplayOrder = 3}
                //new Category { Category_Id = 4, Name = "Drama", DisplayOrder = 4 },   // Add this
                //new Category { Category_Id = 9, Name = "Adventure", DisplayOrder = 5 }
                );

            modelBuilder.Entity<Company>().HasData(
               new Company
               {
                   Id = 1,
                   Name = "TechNova Solutions",
                   StreetAddress = "123 Innovation Drive",
                   City = "San Francisco",
                   State = "CA",
                   PostalCode = "94107",
                   PhoneNumber = "415-555-1234"
               },
               new Company
               {
                   Id = 2,
                   Name = "GreenField Analytics",
                   StreetAddress = "456 Market Street",
                   City = "Seattle",
                   State = "WA",
                   PostalCode = "98101",
                   PhoneNumber = "206-555-5678"
               },
               new Company
               {
                   Id = 3,
                   Name = "SkyHigh Technologies",
                   StreetAddress = "789 Skyline Blvd",
                   City = "Denver",
                   State = "CO",
                   PostalCode = "80202",
                   PhoneNumber = "303-555-7890"
               },
               new Company
               {
                   Id = 4,
                   Name = "UrbanSoft Inc.",
                   StreetAddress = "321 Downtown Ave",
                   City = "Chicago",
                   State = "IL",
                   PostalCode = "60606",
                   PhoneNumber = "312-555-4321"
               },
               new Company
               {
                   Id = 5,
                   Name = "NextGen Systems",
                   StreetAddress = "654 Future Lane",
                   City = "Austin",
                   State = "TX",
                   PostalCode = "73301",
                   PhoneNumber = "512-555-2468"
               }
             );


            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Title = "Fortune of Time",
                    Author = "Billy Spark",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ISBN = "SWD9999001",
                    ListPrice = 99,
                    Price = 90,
                    Price50 = 85,
                    Price100 = 80,
                    Category_Id = 3,
                    ImageUrl = ""
                },
                new Product
                {
                    Id = 2,
                    Title = "Dark Skies",
                    Author = "Nancy Hoover",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ISBN = "CAW777777701",
                    ListPrice = 40,
                    Price = 30,
                    Price50 = 25,
                    Price100 = 20,
                    Category_Id = 4,
                    ImageUrl = ""
                },
                new Product
                {
                    Id = 3,
                    Title = "Vanish in the Sunset",
                    Author = "Julian Button",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ISBN = "RITO5555501",
                    ListPrice = 55,
                    Price = 50,
                    Price50 = 40,
                    Price100 = 35,
                    Category_Id = 9,
                    ImageUrl = ""
                },
                new Product
                {
                    Id = 4,
                    Title = "Cotton Candy",
                    Author = "Abby Muscles",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ISBN = "WS3333333301",
                    ListPrice = 70,
                    Price = 65,
                    Price50 = 60,
                    Price100 = 55,
                    Category_Id = 3,
                    ImageUrl = ""
                },
                new Product
                {
                    Id = 5,
                    Title = "Rock in the Ocean",
                    Author = "Ron Parker",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ISBN = "SOTJ1111111101",
                    ListPrice = 30,
                    Price = 27,
                    Price50 = 25,
                    Price100 = 20,
                    Category_Id = 3,
                    ImageUrl = ""
                },
                new Product
                {
                    Id = 6,
                    Title = "Leaves and Wonders",
                    Author = "Laura Phantom",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ISBN = "FOT000000001",
                    ListPrice = 25,
                    Price = 23,
                    Price50 = 22,
                    Price100 = 20,
                    Category_Id = 3,
                    ImageUrl = ""
                }
                );
        }
    }
}
