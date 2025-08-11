using OrderManagement.Core.Domain.Entities;
using OrderManagement.Core.Domain.RepoContracts;
using OrderManagement.Core.DTOs.OrderDTO;
using OrderManagement.Core.DTOs.OrderItemDTO;
using OrderManagement.Core.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderManagement.Core.Services
{
    public class OrderService(IOrderRepoContract orderRepo) : IOrderService
    {
        private readonly IOrderRepoContract _orderRepo = orderRepo;

        public async Task<OrderResponse> AddOrderAsync(OrderAddRequest? orderRequest)
        {

            ArgumentNullException.ThrowIfNull(orderRequest);
            // Convert request to entity
            var order = new Order
            {
                OrderId = Guid.NewGuid(),
                OrderDate = DateTime.UtcNow,
                CustomerName = orderRequest.CustomerName ?? throw new ArgumentException("Customer name is required"),
                OrderItems = []
            };

           

            // Add to repository
            var addedOrder = await _orderRepo.AddOrderAsync(order);

            // Convert to response
            return addedOrder.ToOrderResponse();
        }

        public async Task<bool> DeleteOrderAsync(Guid? orderId)
        {
            ArgumentNullException.ThrowIfNull(orderId, nameof(orderId));
            return await _orderRepo.DeleteOrderAsync(orderId.Value);
        }

        public async Task<IEnumerable<OrderResponse>?> GetAllOrdersAsync()
        {

            var orders = await _orderRepo.GetAllOrdersAsync();
            return orders?.Select(o=>o.ToOrderResponse());
        }

        public async Task<OrderResponse?> GetOrderByIdAsync(Guid? orderId)
        {
            ArgumentNullException.ThrowIfNull(orderId, nameof(orderId));
            var order = await _orderRepo.GetOrderByIdAsync(orderId.Value);
            return order?.ToOrderResponse();
        }

        public async Task<OrderResponse> UpdateOrderAsync(OrderUpdateRequest? orderUpdateRequest)
        {
            ArgumentNullException.ThrowIfNull(orderUpdateRequest);
            // Get existing order
            var existingOrder = await _orderRepo.GetOrderByIdAsync(orderUpdateRequest.OrderId) ?? throw new KeyNotFoundException($"Order with ID {orderUpdateRequest.OrderId} not found");

            // Update properties
            existingOrder.CustomerName = orderUpdateRequest.CustomerName ?? existingOrder.CustomerName;

            // Update order items if provided
            if (orderUpdateRequest.OrderItems != null)
            {
                existingOrder.OrderItems = [.. orderUpdateRequest.OrderItems
                    .Select(oi => new OrderItem
                    {
                        OrderItemId = oi.OrderItemId != Guid.Empty ? oi.OrderItemId : Guid.NewGuid(),
                        ItemId = oi.ItemId,
                        ItemName = oi.ItemName,
                        Quantity = oi.Quantity,
                        UnitPrice = oi.UnitPrice
                    })];
            }

            // Save changes
            var updatedOrder = await _orderRepo.UpdateOrderAsync(existingOrder);

            // Convert to response
            return updatedOrder.ToOrderResponse();
        }

    }

}