using OrderManagement.Core.Domain.Entities;
using OrderManagement.Core.Domain.RepoContracts;
using OrderManagement.Core.DTOs.OrderItemDTO;
using OrderManagement.Core.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderManagement.Core.Services
{
    public class OrderItemService(IOrderItemRepoContract orderItemRepo, IItemRepoContract itemRepo) : IOrderItemService
    {
        private readonly IOrderItemRepoContract _orderItemRepo = orderItemRepo;
        private readonly IItemRepoContract _itemRepo = itemRepo;

        public async Task<OrderItemResponse> AddOrderItemAsync(OrderItemAddRequest? orderItemRequest)
        {
            ArgumentNullException.ThrowIfNull(orderItemRequest);

            // Validate the item exists
            Item? item = await _itemRepo.GetItemByIdAsync(orderItemRequest.ItemId) ?? throw new KeyNotFoundException($"Item with ID {orderItemRequest.ItemId} not found");

            // Create new order item
            var orderItem = new OrderItem
            {
                OrderItemId = Guid.NewGuid(),
                OrderId = orderItemRequest.OrderId,
                ItemId = orderItemRequest.ItemId,
                ItemName = item.ItemName,
                Quantity = orderItemRequest.Quantity,
                UnitPrice = item.ItemPrice // Use current item price
            };

            // Add to repository
            var addedItem = await _orderItemRepo.AddOrderItemAsync(orderItem);

            // Convert to response
            return addedItem.ToOrderItemResponse();
        }

        public async Task<bool> DeleteOrderItemAsync(Guid? orderItemId)
        {
            ArgumentNullException.ThrowIfNull(orderItemId);
           return await _orderItemRepo.DeleteOrderItemAsync(orderItemId.Value);

        }

        public async Task<IEnumerable<OrderItemResponse>?> GetAllOrderItemsAsync()
        {
            var orderItems = await _orderItemRepo.GetAllOrderItemsAsync();
            return orderItems?.Select(oi=>oi.ToOrderItemResponse());
        }

        public async Task<OrderItemResponse?> GetOrderItemByIdAsync(Guid? orderItemId)
        {
            if (orderItemId == null)
            {
                throw new ArgumentNullException(nameof(orderItemId), "Order item ID cannot be null");
            }
            var orderItem = await _orderItemRepo.GetOrderItemByIdAsync(orderItemId.Value);
            return orderItem?.ToOrderItemResponse();
        }

        public async Task<OrderItemResponse> UpdateOrderItemAsync(OrderItemUpdateRequest? orderItemUpdateRequest)
        {
            ArgumentNullException.ThrowIfNull(orderItemUpdateRequest);

            // Get existing order item
            var existingOrderItem = await _orderItemRepo.GetOrderItemByIdAsync(orderItemUpdateRequest.OrderItemId) ?? throw new KeyNotFoundException($"Order item with ID {orderItemUpdateRequest.OrderItemId} not found");

            // Update properties

            existingOrderItem.Quantity = orderItemUpdateRequest.Quantity;
                existingOrderItem.UnitPrice = orderItemUpdateRequest.UnitPrice;

            // Save changes
            var updatedItem = await _orderItemRepo.UpdateOrderItemAsync(existingOrderItem);

            // Convert to response
            return updatedItem.ToOrderItemResponse();
        }

    }
}