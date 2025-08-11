using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Core.DTOs.ItemDTO;
using OrderManagement.Core.ServiceContracts;
using OrderManagement.Core.Services;

namespace OrderManagement.API.Controllers.v1
{
    /// <summary>
    /// For managing items in the system.
    /// </summary>
    /// 
    [ApiVersion("1.0")]
    public class ItemsController(IItemService itemService, ILogger<ItemsController> logger) : CustomHelperController
    {
        private readonly IItemService _itemService = itemService;
        private readonly ILogger<ItemsController> _logger = logger;

        /// <summary>
        /// Retrieves all items from the system
        /// </summary>
        /// <returns>A list of all items</returns>

        // GET: api/items
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemResponse>>> GetItems()
        {
            try
            {
                var items = await _itemService.GetAllItemsAsync();
                return Ok(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all items");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Retrieves a specific item by its unique identifier
        /// </summary>
        /// <param name="id">The unique identifier of the item to retrieve</param>
        /// <returns>The requested item if found; otherwise returns NotFound</returns>

        // GET: api/items/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemResponse>> GetItem(Guid id)
        {
            try
            {
                var item = await _itemService.GetItemByIdAsync(id);

                if (item == null)
                {
                    return Problem(detail:"This id NotFound",statusCode:404,title: "Getting Item By Id");
                }

                return Ok(item);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting item with ID {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Creates a new item in the system
        /// </summary>
        /// <param name="itemDto">The item data to create</param>
        /// <returns>The newly created item with its generated identifier</returns>

        // POST: api/items
        [HttpPost]
        public async Task<ActionResult<ItemResponse>> PostItem([FromBody] ItemAddRequest itemDto)
        {
            try
            {
                
                var createdItem = await _itemService.AddItemAsync(itemDto);

                return CreatedAtAction(nameof(GetItem),
                    new { id = createdItem.ItemId },
                    createdItem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating new item");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Updates an existing item with the provided data
        /// </summary>
        /// <param name="id">The unique identifier of the item to update</param>
        /// <param name="itemDto">The updated item data</param>
        /// <returns>NoContent if successful; BadRequest if ID mismatch; NotFound if item doesn't exist</returns>

        // PUT: api/items/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutItem(Guid id, [FromBody] ItemUpdateRequest itemDto)
        {
            try
            {
                if (id != itemDto.ItemId)
                    return BadRequest("ID mismatch");

                var result = await _itemService.UpdateItemAsync(itemDto);

                if (result == null)
                    return NotFound($"Item with ID {itemDto.ItemId} not found");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating item with ID {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Deletes an item with the specified unique identifier
        /// </summary>
        /// <param name="id">The unique identifier of the item to delete</param>
        /// <returns>NoContent if successful; NotFound if item doesn't exist</returns>

        // DELETE: api/items/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(Guid? id)
        {
            try
            {
                var result = await _itemService.DeleteItemAsync(id);

                if (!result)
                    return Problem(detail: "This id NotFound", statusCode: 404, title: "Deleteing Item By Id");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting item with ID {id}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
