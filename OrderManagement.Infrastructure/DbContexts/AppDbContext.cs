using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Core.Domain.Entities;
using OrderManagement.Core.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Infrastructure.DbContexts
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<AppUser,AppRole,Guid>(options)
    {
        // DbSets for your entities go here
        public virtual DbSet<OrderItem> OrderItems { get; set; }
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Order> Addresses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configure entity relationships and constraints here

            modelBuilder.Entity<Order>().ToTable("Orders");
            modelBuilder.Entity<OrderItem>().ToTable("OrderItems");
            modelBuilder.Entity<Item>().ToTable("Items");
            modelBuilder.Entity<Address>().ToTable("Addresses");


            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Item)
                .WithMany(i => i.OrderItems)
                .HasForeignKey(i => i.ItemId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Item>()
                .HasMany(i => i.OrderItems)
                .WithOne(oi => oi.Item)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Item>()
                .Property(i => i.ItemPrice)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<OrderItem>()
                .Property(i => i.UnitPrice)
                .HasColumnType("decimal(18,2)");


            // Create a sequence that resets each year
            modelBuilder.HasSequence<int>("OrderNumberSequence");

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(o => o.OrderNumber)
                    .HasDefaultValueSql("CONCAT('Order_', YEAR(GETDATE()), '_', NEXT VALUE FOR dbo.OrderNumberSequence)");
            });


            // Seed Items
            // Use static GUIDs and dates for seeding
            var laptopId = Guid.Parse("71721afe-3d15-4e24-bbf0-d19d90130b6d");
            var phoneId = Guid.Parse("d74d4ef8-4efa-4865-9f15-52243c3320f7");
            var airpodsId = Guid.Parse("65811328-1c98-4f2d-ba52-17cc34285225");
            var keyboardId = Guid.Parse("d60609e0-7915-4dc1-808b-c9f31b2d8b50");
            var mouseId = Guid.Parse("8b241257-bfc6-4fdb-ba3c-d5cbce698fd9");

            var order1Id = Guid.Parse("4af1776a-a87f-4f76-9418-18d84002e439");
            var order2Id = Guid.Parse("27786645-a44a-494b-89ca-c30588ddc137");

            var orderItem1Id = Guid.Parse("12eb0d68-3d4f-4378-abb4-767720838a6e");
            var orderItem2Id = Guid.Parse("b38cbdb5-fb2e-4e77-acc2-6f23e09936d3");
            var orderItem3Id = Guid.Parse("061d2367-e16b-4d23-9ece-e1a287f7f615");
            var orderItem4Id = Guid.Parse("86075d69-54e4-4a96-b474-7de4b6215c5b");

            // Use fixed date instead of DateTime.Now
            var order1Date = new DateTime(2023, 1, 15);
            var order2Date = new DateTime(2023, 1, 18);

            modelBuilder.Entity<Item>().HasData(
                new Item { ItemId = laptopId, ItemName = "Laptop Omine", ItemPrice = 999.99m, ItemTotalQuantity = 50 },
                new Item { ItemId = phoneId, ItemName = "IPhone15", ItemPrice = 699.99m, ItemTotalQuantity = 100 },
                new Item { ItemId = airpodsId, ItemName = "Air Pods", ItemPrice = 149.99m, ItemTotalQuantity = 200 },
                new Item { ItemId = keyboardId, ItemName = "Keyboard Logitech 250x4b4", ItemPrice = 79.99m, ItemTotalQuantity = 150 },
                new Item { ItemId = mouseId, ItemName = "Mouse Copra M11", ItemPrice = 29.99m, ItemTotalQuantity = 300 }
            );

            modelBuilder.Entity<Order>().HasData(
                new Order { OrderId = order1Id, OrderDate = order1Date, CustomerName = "John Doe" },
                new Order { OrderId = order2Id, OrderDate = order2Date, CustomerName = "Jane Smith" }
            );

            modelBuilder.Entity<OrderItem>().HasData(
                new OrderItem { OrderItemId = orderItem1Id, OrderId = order1Id, ItemId = laptopId, ItemName = "Laptop Omine", Quantity = 2, UnitPrice = 999.99m },
                new OrderItem { OrderItemId = orderItem2Id, OrderId = order1Id, ItemId = keyboardId, ItemName = "Keyboard Logitech 250x4b4", Quantity = 2, UnitPrice = 79.99m },
                new OrderItem { OrderItemId = orderItem3Id, OrderId = order1Id, ItemId = phoneId, ItemName = "IPhone15", Quantity = 1, UnitPrice = 699.99m },
                new OrderItem { OrderItemId = orderItem4Id, OrderId = order2Id, ItemId = phoneId, ItemName = "IPhone15", Quantity = 1, UnitPrice = 699.99m }
            );
        }
        
        public async Task EnsureSequenceResetAsync()
        {
            var currentYear = DateTime.Now.Year;

            await Database.ExecuteSqlAsync($"""
             DECLARE @year NVARCHAR(4) = {currentYear};
        
                -- Reset sequence if year changed
             IF NOT EXISTS (
                 SELECT 1 FROM sys.extended_properties 
                 WHERE name = 'OrderSequenceYear' AND value = @year
             )
             BEGIN
                 ALTER SEQUENCE dbo.OrderNumberSequence RESTART WITH 1;
                 
                 -- Update or add extended property
                 IF EXISTS (
                     SELECT 1 FROM sys.extended_properties 
                     WHERE name = 'OrderSequenceYear'
                 )
                     EXEC sp_updateextendedproperty 'OrderSequenceYear', @year, 'SCHEMA', 'dbo';
                 ELSE
                     EXEC sp_addextendedproperty 'OrderSequenceYear', @year, 'SCHEMA', 'dbo';
             END
             """);
        }
    }

}

