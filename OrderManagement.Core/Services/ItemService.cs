using OrderManagement.Core.Domain.Entities;
using OrderManagement.Core.Domain.RepoContracts;
using OrderManagement.Core.DTOs.ItemDTO;
using OrderManagement.Core.DTOs.OrderItemDTO;
using OrderManagement.Core.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderManagement.Core.Services
{
    public class ItemService(IItemRepoContract itemRepoContract) : IItemService
    {
        private readonly IItemRepoContract _itemRepoService = itemRepoContract;

        public async Task<ItemResponse> AddItemAsync(ItemAddRequest? itemRequest)
        {
            ArgumentNullException.ThrowIfNull(itemRequest, nameof(itemRequest));
            // Convert request to entity
            var item = new Item
            {
                ItemId = Guid.NewGuid(),
                ItemName = itemRequest.ItemName,
                ItemPrice = itemRequest.ItemPrice,
                ItemTotalQuantity = itemRequest.ItemTotalQuantity
            };

            // Add to repository
            var addedItem = await _itemRepoService.AddItemAsync(item);

            // Convert to response
            return new ItemResponse
            {
                ItemId = addedItem.ItemId,
                ItemName = addedItem.ItemName,
                ItemPrice = addedItem.ItemPrice,
                ItemTotalQuantity = addedItem.ItemTotalQuantity
            };
        }

        public async Task<bool> DeleteItemAsync(Guid? itemId)
        {
            ArgumentNullException.ThrowIfNull(itemId, nameof(itemId));
            return await _itemRepoService.DeleteItemAsync(itemId.Value);
        }

        public async Task<IEnumerable<ItemResponse>?> GetAllItemsAsync()
        {  
            var items = await _itemRepoService.GetAllItemsAsync();
            return items.Select(item => item.ToItemResponse());
        }

        public async Task<ItemResponse?> GetItemByIdAsync(Guid? itemId)
        {
            ArgumentNullException.ThrowIfNull(itemId, nameof(itemId));
            var item = await _itemRepoService.GetItemByIdAsync(itemId.Value);
            if (item == null) return null;

            return new ItemResponse
            {
                ItemId = item.ItemId,
                ItemName = item.ItemName,
                ItemPrice = item.ItemPrice,
                ItemTotalQuantity = item.ItemTotalQuantity
            };
        }

        public async Task<ItemResponse?> UpdateItemAsync(ItemUpdateRequest? itemUpdateRequest)
        {
            ArgumentNullException.ThrowIfNull(itemUpdateRequest, nameof(itemUpdateRequest));
            // Get existing item
            var existingItem = await _itemRepoService.GetItemByIdAsync(itemUpdateRequest.ItemId);
            if (existingItem == null)
            {
                return null; // Item not found
            }

            // Update properties only if new values are provided

            if(itemUpdateRequest.ItemName != null)
            existingItem.ItemName = itemUpdateRequest.ItemName;

            if(itemUpdateRequest.ItemPrice != null)
                existingItem.ItemPrice = itemUpdateRequest.ItemPrice;
            if(itemUpdateRequest.ItemTotalQuantity != null)
                existingItem.ItemTotalQuantity = itemUpdateRequest.ItemTotalQuantity;
        

            // Save changes
            var updatedItem = await _itemRepoService.UpdateItemAsync(existingItem);

            // Convert to response
            return new ItemResponse
            {
                ItemId = updatedItem.ItemId,
                ItemName = updatedItem.ItemName,
                ItemPrice = updatedItem.ItemPrice,
                ItemTotalQuantity = updatedItem.ItemTotalQuantity
            };
        }
    }
}