using OrderManagement.Core.Domain.Entities;

namespace OrderManagement.Core.Domain.RepoContracts
{
    public interface IOrderItemRepoContract
    {
        /// <summary>
        /// Adds a new Order Item to the repository.
        /// </summary>
        /// <param name="orderItem">The Order Item to add.</param>
        /// <returns>The added Order Item.</returns>
        Task<OrderItem> AddOrderItemAsync(OrderItem orderItem);
        /// <summary>
        /// Retrieves an Order Item by its ID.
        /// </summary>
        /// <param name="orderItemId">The ID of the Order Item to retrieve.</param>
        /// <returns>The Order Item with the specified ID, or null if not found.</returns>
        Task<OrderItem?> GetOrderItemByIdAsync(Guid orderItemId);
        /// <summary>
        /// Retrieves all Order items in the repository.
        /// </summary>
        /// <returns>A list of all Order items.</returns>
        Task<IEnumerable<OrderItem>> GetAllOrderItemsAsync();
        /// <summary>
        /// Updates an existing Order Item in the repository.
        /// </summary>
        /// <param name="orderItem">The Order Item to update.</param>
        /// <returns>The updated Order Item.</returns>
        Task<OrderItem> UpdateOrderItemAsync(OrderItem orderItem);
        /// <summary>
        /// Deletes an Order Item from the repository.
        /// </summary>
        /// <param name="orderItemId">The ID of the Order Item to delete.</param>
        Task<bool> DeleteOrderItemAsync(Guid orderItemId);
    }
}
