using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShopApi.Data.Models;
using System;

namespace ShopApi.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<ShippingDetail> ShippingDetails { get; set; }


        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
            // when the app starts, checks if any changes need to be made to the DB and updates it if necessary.
            // Without docker, we normally do this on the command line: update-database
            // We cant do that here because we need SQL SERVER docker container to be runnning before trying to update it.
            // However, we can probably create this via command line as part of publishing our application
            //if (this.Database.IsSqlServer())
            //{
            //    this.Database.Migrate();
            //    //SeedData();
            //}
        }

        private void SeedData()
        {
            string sql1 = @"
                SET IDENTITY_INSERT [dbo].[Categories] ON
                INSERT INTO [dbo].[Categories] ([CategoryId], [Name]) VALUES (1, N'Bread')
                INSERT INTO [dbo].[Categories] ([CategoryId], [Name]) VALUES (2, N'Dairy')
                INSERT INTO [dbo].[Categories] ([CategoryId], [Name]) VALUES (3, N'Fruits')
                INSERT INTO [dbo].[Categories] ([CategoryId], [Name]) VALUES (4, N'Seasonings')
                INSERT INTO [dbo].[Categories] ([CategoryId], [Name]) VALUES (5, N'Vegetables')
                SET IDENTITY_INSERT [dbo].[Categories] OFF
            ";

            this.Database.ExecuteSqlRaw(sql1);

            string sql = @"
                SET IDENTITY_INSERT [dbo].[Products] ON
                INSERT INTO [dbo].[Products] ([ProductId], [Name], [CategoryId], [ImageUrl], [Price]) VALUES (1, N'Freshly Baked Bread', 1, N'https://upload.wikimedia.org/wikipedia/commons/7/71/Anadama_bread_%281%29.jpg', CAST(4.00 AS Decimal(18, 2)))
                INSERT INTO [dbo].[Products] ([ProductId], [Name], [CategoryId], [ImageUrl], [Price]) VALUES (2, N'Spinach', 5, N'https://www.publicdomainpictures.net/pictures/170000/velka/spinach-leaves-1461774375kTU.jpg', CAST(2.50 AS Decimal(18, 2)))
                INSERT INTO [dbo].[Products] ([ProductId], [Name], [CategoryId], [ImageUrl], [Price]) VALUES (4, N'Avocado', 3, N'https://images.unsplash.com/photo-1583029901628-8039767c7ad0?ixid=MXwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHw%3D&ixlib=rb-1.2.1&auto=format&fit=crop&w=1350&q=80', CAST(1.75 AS Decimal(18, 2)))
                INSERT INTO [dbo].[Products] ([ProductId], [Name], [CategoryId], [ImageUrl], [Price]) VALUES (5, N'Tomato', 5, N'https://static.pexels.com/photos/8390/food-wood-tomatoes.jpg', CAST(2.50 AS Decimal(18, 2)))
                INSERT INTO [dbo].[Products] ([ProductId], [Name], [CategoryId], [ImageUrl], [Price]) VALUES (6, N'Lettuce', 5, N'https://upload.wikimedia.org/wikipedia/commons/7/7f/Lettuce_Mini_Heads_%287331119710%29.jpg', CAST(1.00 AS Decimal(18, 2)))
                INSERT INTO [dbo].[Products] ([ProductId], [Name], [CategoryId], [ImageUrl], [Price]) VALUES (7, N'Cauliflower', 5, N'https://upload.wikimedia.org/wikipedia/commons/thumb/1/11/Cauliflowers_-_20051021.jpg/1280px-Cauliflowers_-_20051021.jpg', CAST(1.75 AS Decimal(18, 2)))
                INSERT INTO [dbo].[Products] ([ProductId], [Name], [CategoryId], [ImageUrl], [Price]) VALUES (8, N'Banana', 3, N'https://upload.wikimedia.org/wikipedia/commons/thumb/4/4c/Bananas.jpg/1024px-Bananas.jpg', CAST(1.25 AS Decimal(18, 2)))
                INSERT INTO [dbo].[Products] ([ProductId], [Name], [CategoryId], [ImageUrl], [Price]) VALUES (9, N'Orange', 3, N'https://upload.wikimedia.org/wikipedia/commons/c/c4/Orange-Fruit-Pieces.jpg', CAST(1.70 AS Decimal(18, 2)))
                INSERT INTO [dbo].[Products] ([ProductId], [Name], [CategoryId], [ImageUrl], [Price]) VALUES (10, N'Apple', 3, N'https://upload.wikimedia.org/wikipedia/commons/1/15/Red_Apple.jpg', CAST(2.00 AS Decimal(18, 2)))
                INSERT INTO [dbo].[Products] ([ProductId], [Name], [CategoryId], [ImageUrl], [Price]) VALUES (11, N'Grape', 3, N'https://upload.wikimedia.org/wikipedia/commons/3/36/Kyoho-grape.jpg', CAST(2.00 AS Decimal(18, 2)))
                INSERT INTO [dbo].[Products] ([ProductId], [Name], [CategoryId], [ImageUrl], [Price]) VALUES (12, N'Peach', 3, N'https://upload.wikimedia.org/wikipedia/commons/9/9e/Autumn_Red_peaches.jpg', CAST(2.00 AS Decimal(18, 2)))
                INSERT INTO [dbo].[Products] ([ProductId], [Name], [CategoryId], [ImageUrl], [Price]) VALUES (13, N'Cinnamon Sticks', 4, N'https://upload.wikimedia.org/wikipedia/commons/8/8c/Cinnamon-other.jpg', CAST(0.50 AS Decimal(18, 2)))
                INSERT INTO [dbo].[Products] ([ProductId], [Name], [CategoryId], [ImageUrl], [Price]) VALUES (14, N'Saffron', 4, N'https://upload.wikimedia.org/wikipedia/commons/4/48/Saffron_Crop.JPG', CAST(3.00 AS Decimal(18, 2)))
                INSERT INTO [dbo].[Products] ([ProductId], [Name], [CategoryId], [ImageUrl], [Price]) VALUES (15, N'Ground Turmeric', 4, N'http://maxpixel.freegreatpicture.com/static/photo/1x/Seasoning-Powder-Curry-Spice-Ingredient-Turmeric-2344157.jpg', CAST(0.75 AS Decimal(18, 2)))
                INSERT INTO [dbo].[Products] ([ProductId], [Name], [CategoryId], [ImageUrl], [Price]) VALUES (16, N'Coriander Seeds', 4, N'https://cdn.pixabay.com/photo/2014/07/11/22/53/coriander-390708_960_720.jpg', CAST(0.50 AS Decimal(18, 2)))
                INSERT INTO [dbo].[Products] ([ProductId], [Name], [CategoryId], [ImageUrl], [Price]) VALUES (17, N'Lavash Bread', 1, N'https://upload.wikimedia.org/wikipedia/commons/thumb/4/43/Fabrication_du_lavash_%C3%A0_Noravank_%286%29.jpg/1280px-Fabrication_du_lavash_%C3%A0_Noravank_%286%29.jpg', CAST(1.25 AS Decimal(18, 2)))
                INSERT INTO [dbo].[Products] ([ProductId], [Name], [CategoryId], [ImageUrl], [Price]) VALUES (18, N'Bagel Bread', 1, N'https://upload.wikimedia.org/wikipedia/commons/1/1d/Bagel-Plain-Alt.jpg', CAST(1.00 AS Decimal(18, 2)))
                INSERT INTO [dbo].[Products] ([ProductId], [Name], [CategoryId], [ImageUrl], [Price]) VALUES (19, N'Baguette Bread', 1, N'https://static.pexels.com/photos/416607/pexels-photo-416607.jpeg', CAST(1.25 AS Decimal(18, 2)))
                INSERT INTO [dbo].[Products] ([ProductId], [Name], [CategoryId], [ImageUrl], [Price]) VALUES (20, N'Strawberry', 3, N'https://upload.wikimedia.org/wikipedia/commons/e/e1/Strawberries.jpg', CAST(1.95 AS Decimal(18, 2)))
                SET IDENTITY_INSERT [dbo].[Products] OFF
            ";

            this.Database.ExecuteSqlRaw(sql);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            SetupRelationships(builder);
        }

        private void SetupRelationships(ModelBuilder builder)
        {
            // each AppUser can have many UserClaims
            builder.Entity<AppUser>()
                   .HasMany(x => x.Claims)
                   .WithOne()
                   .HasForeignKey(x => x.UserId)
                   .IsRequired();
        }
    }
}
