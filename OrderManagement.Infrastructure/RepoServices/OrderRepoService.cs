using Microsoft.EntityFrameworkCore;
using OrderManagement.Core.Domain.Entities;
using OrderManagement.Core.Domain.RepoContracts;
using OrderManagement.Infrastructure.DbContexts;

namespace OrderManagement.Infrastructure.RepoServices
{
    public class OrderRepoService(AppDbContext context) : IOrderRepoContract
    {
        private readonly AppDbContext _db = context;

        public async Task<Order> AddOrderAsync(Order order)
        {
            await _db.Orders.AddAsync(order);
            return await _db.SaveChangesAsync().ContinueWith(t => order);
        }

        public async Task<bool> DeleteOrderAsync(Guid orderId)
        {

            _db.Orders.RemoveRange(_db.Orders.Where(i => i.OrderId == orderId));
            int row = await _db.SaveChangesAsync();
            return row > 0; // Deletion successful

        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _db.Orders.Include(i => i.OrderItems).ToListAsync();
        }

        public async Task<Order?> GetOrderByIdAsync(Guid orderId)
        {
            return await _db.Orders.Include(i => i.OrderItems).FirstOrDefaultAsync(o=>o.OrderId==orderId);

        }

        public async Task<Order> UpdateOrderAsync(Order order)
        {
            Order? orderMatching = await _db.Orders.FirstOrDefaultAsync(i => i.OrderId == order.OrderId);
            if (orderMatching == null)
            {
                return order;
            }
            orderMatching.CustomerName = order.CustomerName;
            orderMatching.OrderItems = order.OrderItems;
            await _db.SaveChangesAsync();
            return orderMatching;
        }
    }
    }
