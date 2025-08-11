using OrderManagement.Core.Domain.Entities;
using OrderManagement.Core.DTOs.OrderDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Core.ServiceContracts
{
    public interface IOrderService
    {
        /// <summary>
        /// Adds a new order to the system.
        /// </summary>
        /// <param name="orderRequest">The order details to add.</param>
        /// <returns>The added order.</returns>
        Task<OrderResponse> AddOrderAsync(OrderAddRequest? orderRequest);
        /// <summary>
        /// Retrieves an order by its ID.
        /// </summary>
        /// <param name="orderId">The ID of the order to retrieve.</param>
        /// <returns>The order with the specified ID, or null if not found.</returns>
        Task<OrderResponse?> GetOrderByIdAsync(Guid? orderId);
        /// <summary>
        /// Retrieves all orders in the system.
        /// </summary>
        /// <returns>A list of all orders.</returns>
        Task<IEnumerable<OrderResponse>?> GetAllOrdersAsync();
        /// <summary>
        /// Updates an existing order in the system.
        /// </summary>
        /// <param name="orderUpdateRequest">The updated order details.</param>
        /// <returns>The updated order.</returns>
        Task<OrderResponse> UpdateOrderAsync(OrderUpdateRequest? orderUpdateRequest);
        /// <summary>
        /// Deletes an order from the system.
        /// </summary>
        /// <param name="orderId">The ID of the order to delete.</param>
        Task<bool> DeleteOrderAsync(Guid? orderId);

    }
}
