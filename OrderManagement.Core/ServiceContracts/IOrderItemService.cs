using OrderManagement.Core.DTOs.OrderItemDTO;

namespace OrderManagement.Core.ServiceContracts
{
    public interface IOrderItemService
    {
        /// <summary>
        /// Adds a new orderItem to the system.
        /// </summary>
        /// <param name="orderItemRequest">The orderItem details to add.</param>
        /// <returns>The added orderItem.</returns>
        Task<OrderItemResponse> AddOrderItemAsync(OrderItemAddRequest? orderItemRequest);
        /// <summary>
        /// Retrieves an orderItem by its ID.
        /// </summary>
        /// <param name="orderItemId">The ID of the orderItem to retrieve.</param>
        /// <returns>The orderItemId with the specified ID, or null if not found.</returns>
        Task<OrderItemResponse?> GetOrderItemByIdAsync(Guid? orderItemId);
        /// <summary>
        /// Retrieves all orderitems in the system.
        /// </summary>
        /// <returns>A list of all orderitems.</returns>
        Task<IEnumerable<OrderItemResponse>?> GetAllOrderItemsAsync();
        /// <summary>
        /// Updates an existing orderitems in the system.
        /// </summary>
        /// <param name="orderItemUpdateRequest">The updated orderItem details.</param>
        /// <returns>The updated orderItem.</returns>
        Task<OrderItemResponse> UpdateOrderItemAsync(OrderItemUpdateRequest? orderItemUpdateRequest);
        /// <summary>
        /// Deletes an orderItem from the system.
        /// </summary>
        /// <param name="orderItemId">The ID of the orderItem to delete.</param>
        Task<bool> DeleteOrderItemAsync(Guid? orderItemId);
    }

}