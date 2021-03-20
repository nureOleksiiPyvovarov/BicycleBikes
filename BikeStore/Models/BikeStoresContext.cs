using System;
using System.Collections.Generic;
using BikeStore.Models;
using BikeStore.Models.EfModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BikeStore
{
    public partial class BikeStoresContext : IdentityDbContext<UserIdentityModel, IdentityRole<int>,int>
    {
        public BikeStoresContext(DbContextOptions<BikeStoresContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        public virtual DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
        public virtual DbSet<OrderProduct> OrderProducts { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Brand> Brands { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Store> Stores { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Brand>(entity =>
            {
                entity.ToTable("brands", "production");

                entity.Property(e => e.BrandId).HasColumnName("brand_id");

                entity.Property(e => e.BrandName)
                    .IsRequired()
                    .HasColumnName("brand_name")
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("categories", "production");

                entity.Property(e => e.CategoryId).HasColumnName("category_id");

                entity.Property(e => e.CategoryName)
                    .IsRequired()
                    .HasColumnName("category_name")
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("products", "production");

                entity.Property(e => e.ProductId).HasColumnName("product_id");

                entity.Property(e => e.BrandId).HasColumnName("brand_id");

                entity.Property(e => e.CategoryId).HasColumnName("category_id");

                entity.Property(e => e.ListPrice)
                    .HasColumnName("list_price")
                    .HasColumnType("decimal(10, 2)");

                entity.Property(e => e.ModelYear).HasColumnName("model_year");

                entity.Property(e => e.ProductName)
                    .IsRequired()
                    .HasColumnName("product_name")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ProductPhoto)
                    .HasColumnName("product_photo")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.BrandId)
                    .HasConstraintName("FK__products__brand___286302EC");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK__products__catego__276EDEB3");
            });

            modelBuilder.Entity<Store>(entity =>
            {
                entity.ToTable("stores", "sales");

                entity.Property(e => e.StoreId).HasColumnName("store_id");

                entity.Property(e => e.City)
                    .HasColumnName("city")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .HasColumnName("phone")
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.State)
                    .HasColumnName("state")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.StoreName)
                    .IsRequired()
                    .HasColumnName("store_name")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Street)
                    .HasColumnName("street")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ZipCode)
                    .HasColumnName("zip_code")
                    .HasMaxLength(5)
                    .IsUnicode(false);
            });
            modelBuilder.Entity<Order>(order =>
            {
                order.HasOne(o => o.User)
                    .WithMany(u => u.Orders)
                    .HasForeignKey(o => o.UserId);
            });
            modelBuilder.Entity<OrderProduct>(orderProducts =>
            {
                orderProducts.HasOne(o => o.Order)
                    .WithMany(o => o.Products)
                    .HasForeignKey(or => or.OrderId);
                orderProducts.HasKey(or => new {or.OrderId, or.ProductId});
            });
            modelBuilder.Seed();
        }
    }
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            Console.WriteLine("Standard Numeric Format Specifiers");
            modelBuilder.Entity<UserIdentityModel>().HasData(new UserIdentityModel()
            {
                Id = 1,
                AccessFailedCount = 0,
                ConcurrencyStamp = "32671cd8-02a4-49b3-82e3-be501d168110",
                UserName = "MainAdmin",
                NormalizedUserName = "MAINADMIN",
                Email = "admin@mail.com",
                NormalizedEmail = "ADMIN@MAIL.COM",
                EmailConfirmed = false,
                PasswordHash = "$2a$04$RLJ4XeOeu0f90Wk9MN56NukfV6v2uU99URtHwKPuxbkwzx9gfvKAy",
                SecurityStamp = "AFVCDJLQE5YOFQOZJRK43RC6UYPIVIJD",
                PhoneNumber = "0967583885",
                LockoutEnabled = true
            });
            modelBuilder.Entity<IdentityRole<int>>().HasData(new List<IdentityRole<int>>()
            {
                new IdentityRole<int>()
                {
                    Id = 1,
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    ConcurrencyStamp = "14e03940-8669-42fd-9992-bb93c7abee5e"
                },
                new IdentityRole<int>()
                {
                    Id = 2,
                    Name = "Customer",
                    NormalizedName = "CUSTOMER",
                    ConcurrencyStamp = "4d871a42-9903-48c3-a902-8fc95c8bf67a"
                }
            });
            modelBuilder.Entity<IdentityUserRole<int>>().HasData(new IdentityUserRole<int>()
            {
                UserId = 1,
                RoleId = 1
            });
            modelBuilder.Entity<Brand>().HasData(new List<Brand>()
            {
                new Brand()
                {
                    BrandId = 1,
                    BrandName = "Electra"
                },
                new Brand()
                {
                    BrandId = 2,
                    BrandName = "Haro"
                },
                new Brand()
                {
                    BrandId = 3,
                    BrandName = "Heller"
                },
                new Brand()
                {
                    BrandId = 4,
                    BrandName = "Pure Cycles"
                },
                new Brand()
                {
                    BrandId = 5,
                    BrandName = "Ritchey"
                },
                new Brand()
                {
                    BrandId = 6,
                    BrandName = "Strider"
                },
                new Brand()
                {
                    BrandId = 7,
                    BrandName = "Sun Bicycles"
                },
                new Brand()
                {
                    BrandId = 8,
                    BrandName = "Surly"
                },
                new Brand()
                {
                    BrandId = 9,
                    BrandName = "Trek"
                }
            });
            modelBuilder.Entity<Category>().HasData(new List<Category>()
            {
                new Category()
                {
                    CategoryId = 1,
                    CategoryName = "Children Bicycles"
                },
                new Category()
                {
                    CategoryId = 2,
                    CategoryName = "Comfort Bicycles"
                },
                new Category()
                {
                    CategoryId = 3,
                    CategoryName = "Cruisers Bicycles"
                },
                new Category()
                {
                    CategoryId = 4,
                    CategoryName = "Cyclocross Bicycles"
                },
                new Category()
                {
                    CategoryId = 5,
                    CategoryName = "Electric Bikes"
                },
                new Category()
                {
                    CategoryId = 6,
                    CategoryName = "Mountain Bikes"
                },
                new Category()
                {
                    CategoryId = 7,
                    CategoryName = "Road Bikes"
                }
            });
            modelBuilder.Entity<Product>().HasData(new List<Product>()
            {
                new Product()
                {
                    BrandId = 5,
                    CategoryId = 6,
                    ListPrice = 749.99M,
                    ModelYear = 2019,
                    ProductId = 2,
                    ProductName = "Ritchey Timberwolf Frameset",
                    ProductPhoto = "~/img/bike3.jpg"
                },
                new Product()
                {
                    BrandId = 4,
                    CategoryId = 6,
                    ListPrice = 300.99M,
                    ModelYear = 2019,
                    ProductId = 3,
                    ProductName = "Surly Wednesday Frameset",
                    ProductPhoto = "~/img/bike2.jpg"
                },
                new Product()
                {
                    BrandId = 9,
                    CategoryId = 3,
                    ListPrice = 500.99M,
                    ModelYear = 2019,
                    ProductId = 4,
                    ProductName = "Trek Fuel EX 8 29",
                    ProductPhoto = "~/img/bike1.jpg"
                },
                new Product()
                {
                    BrandId = 3,
                    CategoryId = 4,
                    ListPrice = 700.99M,
                    ModelYear = 2019,
                    ProductId = 5,
                    ProductName = "Heller Shagamaw Frame",
                    ProductPhoto = "~/img/bike5.jpg"
                },
                new Product()
                {
                    BrandId = 9,
                    CategoryId = 4,
                    ListPrice = 450.99M,
                    ModelYear = 2019,
                    ProductId = 6,
                    ProductName = "Trek Slash 8 27.5",
                    ProductPhoto = "~/img/bike4.jpg"
                },
                new Product()
                {
                    BrandId = 4,
                    CategoryId = 3,
                    ListPrice = 320.99M,
                    ModelYear = 2019,
                    ProductId = 7,
                    ProductName = "Trek Remedy 29 Carbon Frameset",
                    ProductPhoto = "~/img/bike1.jpg"
                },
                new Product()
                {
                    BrandId = 9,
                    CategoryId = 5,
                    ListPrice = 200.99M,
                    ModelYear = 2019,
                    ProductId = 8,
                    ProductName = "Trek Conduit+",
                    ProductPhoto = "~/img/bike2.jpg"
                },
                new Product()
                {
                    BrandId = 8,
                    CategoryId = 4,
                    ListPrice = 400M,
                    ModelYear = 2019,
                    ProductId = 9,
                    ProductName = "Surly Straggler",
                    ProductPhoto = "~/img/bike5.jpg"
                },
                new Product()
                {
                    BrandId = 8,
                    CategoryId = 3,
                    ListPrice = 749.99M,
                    ModelYear = 2019,
                    ProductId = 10,
                    ProductName = "Surly Straggler 650b",
                    ProductPhoto = "~/img/bike3.jpg"
                },
                new Product()
                {
                    BrandId = 5,
                    CategoryId = 4,
                    ListPrice = 350.99M,
                    ModelYear = 2019,
                    ProductId = 11,
                    ProductName = "Pure Cycles Western 3-Speed - Women's",
                    ProductPhoto = "~/img/bike1.jpg"
                },
                new Product()
                {
                    BrandId = 1,
                    CategoryId = 3,
                    ListPrice = 590.99M,
                    ModelYear = 2019,
                    ProductId = 12,
                    ProductName = "Pure Cycles Vine 8-Speed",
                    ProductPhoto = "~/img/bike5.jpg"

                }
            });
        }
    }
}
