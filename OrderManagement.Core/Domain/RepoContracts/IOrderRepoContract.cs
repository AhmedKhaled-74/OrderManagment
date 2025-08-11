using OrderManagement.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Core.Domain.RepoContracts
{
   public interface IOrderRepoContract
   {
        /// <summary>
        /// Adds a new Order to the repository.
        /// </summary>
        /// <param name="order">The Order to add.</param>
        /// <returns>The added Order.</returns>
        Task<Order> AddOrderAsync(Order order);
        /// <summary>
        /// Retrieves an Order by its ID.
        /// </summary>
        /// <param name="orderId">The ID of the Order to retrieve.</param>
        /// <returns>The Order with the specified ID, or null if not found.</returns>
        Task<Order?> GetOrderByIdAsync(Guid orderId);
        /// <summary>
        /// Retrieves all orders in the repository.
        /// </summary>
        /// <returns>A list of all orders.</returns>
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        /// <summary>
        /// Updates an existing Order in the repository.
        /// </summary>
        /// <param name="order">The Order to update.</param>
        /// <returns>The updated Order.</returns>
        Task<Order> UpdateOrderAsync(Order order);
        /// <summary>
        /// Deletes an Order from the repository.
        /// </summary>
        /// <param name="orderId">The ID of the Order to delete.</param>
        Task<bool> DeleteOrderAsync(Guid orderId);
    }
}
