using OrderManagement.Core.Domain.Entities;

namespace OrderManagement.Core.Domain.RepoContracts
{
    public interface IItemRepoContract
    {
        /// <summary>
        /// Adds a new Item to the repository.
        /// </summary>
        /// <param name="item">The Item to add.</param>
        /// <returns>The added Item.</returns>
        Task<Item> AddItemAsync(Item item);
        /// <summary>
        /// Retrieves an Item by its ID.
        /// </summary>
        /// <param name="itemId">The ID of the Item to retrieve.</param>
        /// <returns>The Item with the specified ID, or null if not found.</returns>
        Task<Item?> GetItemByIdAsync(Guid itemId);
        /// <summary>
        /// Retrieves all items in the repository.
        /// </summary>
        /// <returns>A list of all items.</returns>
        Task<IEnumerable<Item>> GetAllItemsAsync();
        /// <summary>
        /// Updates an existing Item in the repository.
        /// </summary>
        /// <param name="item">The Item to update.</param>
        /// <returns>The updated Item.</returns>
        Task<Item> UpdateItemAsync(Item item);
        /// <summary>
        /// Deletes an Item from the repository.
        /// </summary>
        /// <param name="itemId">The ID of the Item to delete.</param>
        Task<bool> DeleteItemAsync(Guid itemId);
    }
}
