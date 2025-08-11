using OrderManagement.Core.DTOs.ItemDTO;

namespace OrderManagement.Core.ServiceContracts
{
    public interface IItemService
    {
        /// <summary>
        /// Adds a new item to the system.
        /// </summary>
        /// <param name="itemRequest">The item details to add.</param>
        /// <returns>The added item.</returns>
        Task<ItemResponse> AddItemAsync(ItemAddRequest? itemRequest);
        /// <summary>
        /// Retrieves an item by its ID.
        /// </summary>
        /// <param name="itemId">The ID of the item to retrieve.</param>
        /// <returns>The item with the specified ID, or null if not found.</returns>
        Task<ItemResponse?> GetItemByIdAsync(Guid? itemId);
        /// <summary>
        /// Retrieves all items in the system.
        /// </summary>
        /// <returns>A list of all items.</returns>
        Task<IEnumerable<ItemResponse>?> GetAllItemsAsync();
        /// <summary>
        /// Updates an existing item in the system.
        /// </summary>
        /// <param name="itemUpdateRequest">The updated item details.</param>
        /// <returns>The updated item.</returns>
        Task<ItemResponse?> UpdateItemAsync(ItemUpdateRequest? itemUpdateRequest);
        /// <summary>
        /// Deletes an item from the system.
        /// </summary>
        /// <param name="itemId">The ID of the item to delete.</param>
        Task<bool> DeleteItemAsync(Guid? itemId);
    }

}