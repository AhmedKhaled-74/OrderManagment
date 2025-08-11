using Microsoft.EntityFrameworkCore;
using OrderManagement.Core.Domain.Entities;
using OrderManagement.Core.Domain.RepoContracts;
using OrderManagement.Infrastructure.DbContexts;

namespace OrderManagement.Infrastructure.RepoServices
{
    public class OrderItemRepoService(AppDbContext context) : IOrderItemRepoContract
    {
        private readonly AppDbContext _db = context;

        public async Task<OrderItem> AddOrderItemAsync(OrderItem orderItem)
        {
            await _db.OrderItems.AddAsync(orderItem);
            await _db.SaveChangesAsync();
            return orderItem;
        }

        public async Task<bool> DeleteOrderItemAsync(Guid orderItemId)
        {
            _db.OrderItems.RemoveRange(_db.OrderItems.Where(i => i.OrderItemId == orderItemId));
            int row = await _db.SaveChangesAsync();
            return row > 0; // Deletion successful
        }

        public async Task<IEnumerable<OrderItem>> GetAllOrderItemsAsync()
        {
            return await _db.OrderItems
                .Include(oi => oi.Order)
                .Include(oi => oi.Item)
                .ToListAsync();
        }

        public async Task<OrderItem?> GetOrderItemByIdAsync(Guid orderItemId)
        {
            return await _db.OrderItems
                .Include(oi => oi.Order)
                .Include(oi => oi.Item)
                .FirstOrDefaultAsync(oi => oi.OrderItemId == orderItemId);
        }

        public async Task<OrderItem> UpdateOrderItemAsync(OrderItem orderItem)
        {
            var existingItem = await _db.OrderItems.FindAsync(orderItem.OrderItemId);
            if (existingItem == null)
            {
                return orderItem;
            }

            // Update properties
            existingItem.Quantity = orderItem.Quantity;
            existingItem.UnitPrice = orderItem.UnitPrice;

            await _db.SaveChangesAsync();
            return existingItem;
        }
    }
    }
