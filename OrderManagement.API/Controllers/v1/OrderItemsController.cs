using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Core.DTOs.OrderItemDTO;
using OrderManagement.Core.ServiceContracts;

namespace OrderManagement.API.Controllers.v1
{
    /// <summary>
    /// For managing order items in the system.
    /// </summary>
    /// 
    [ApiVersion("1.0")]
    public class OrderItemsController(IOrderItemService orderItemService, ILogger<OrderItemsController> logger) : CustomHelperController
    {
        private readonly IOrderItemService _orderItemService = orderItemService;
        private readonly ILogger<OrderItemsController> _logger = logger;

        /// <summary>
        /// Retrieves all order items
        /// </summary>
        /// <returns>A list of all order items</returns>
        
        // GET: api/orderItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderItemResponse>>> GetOrderItems()
        {
            try
            {
                var orderItems = await _orderItemService.GetAllOrderItemsAsync();
                return Ok(orderItems);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all orderItems");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Retrieves a specific order item by its unique identifier
        /// </summary>
        /// <param name="id">The unique identifier of the order item to retrieve</param>
        /// <returns>The requested order item if found; otherwise returns NotFound</returns>

        // GET: api/orderItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderItemResponse>> GetOrderItem(Guid id)
        {
            try
            {
                var orderItem = await _orderItemService.GetOrderItemByIdAsync(id);

                if (orderItem == null)
                {
                    return Problem(detail: "This id NotFound", statusCode: 404, title: "Getting OrderItem By Id");
                }

                return Ok(orderItem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting orderItem with ID {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Creates a new order item with the provided data
        /// </summary>
        /// <param name="orderItemDto">The order item data to create</param>
        /// <returns>The newly created order item with its generated identifier</returns>

        // POST: api/orderItems
        [HttpPost]
        public async Task<ActionResult<OrderItemResponse>> PostOrderItem([FromBody] OrderItemAddRequest orderItemDto)
        {
            try
            {
                var createdOrderItem = await _orderItemService.AddOrderItemAsync(orderItemDto);

                return CreatedAtAction(nameof(GetOrderItem),
                    new { id = createdOrderItem.OrderItemId },
                    createdOrderItem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating new orderItem");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Updates an existing order item with the provided data
        /// </summary>
        /// <param name="id">The unique identifier of the order item to update</param>
        /// <param name="orderItemDto">The updated order item data</param>
        /// <returns>NoContent if successful; BadRequest if ID mismatch; NotFound if order item doesn't exist</returns>

        // PUT: api/orderItems/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrderItem(Guid id, [FromBody] OrderItemUpdateRequest orderItemDto)
        {
            try
            {
                if (id != orderItemDto.OrderItemId)
                    return BadRequest("ID mismatch");

                var result = await _orderItemService.UpdateOrderItemAsync(orderItemDto);

                if (result == null)
                    return NotFound($"OrderItem with ID {orderItemDto.OrderItemId} not found");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating orderItem with ID {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Deletes an order item with the specified unique identifier
        /// </summary>
        /// <param name="id">The unique identifier of the order item to delete</param>
        /// <returns>NoContent if successful; NotFound if order item doesn't exist</returns>

        // DELETE: api/orderItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderItem(Guid? id)
        {
            try
            {
                var result = await _orderItemService.DeleteOrderItemAsync(id);
                if (!result)
                    return Problem(detail: "This id NotFound", statusCode: 404, title: "Deleteing OrderItem" + " By Id");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting orderItem with ID {id}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}