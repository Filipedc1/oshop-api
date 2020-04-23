﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShopApi.Data.Models;

namespace ShopApi.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }


        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
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
