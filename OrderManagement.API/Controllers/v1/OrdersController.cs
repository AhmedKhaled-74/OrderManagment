using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Core.DTOs.OrderDTO;
using OrderManagement.Core.ServiceContracts;
using OrderManagement.Core.Services;

namespace OrderManagement.API.Controllers.v1
{
    /// <summary>
    /// For managing orders in the system.
    /// </summary>

    [ApiVersion("1.0")]
    public class OrdersController(IOrderService orderService, ILogger<OrdersController> logger) : CustomHelperController
    {
        private readonly IOrderService _orderService = orderService;
        private readonly ILogger<OrdersController> _logger = logger;
        /// <summary>
        /// Retrieves all orders including their associated order items
        /// </summary>
        /// <returns>A list of all orders with their order items</returns>
        
        // GET: api/orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderResponse>>> GetOrders()
        {
            try
            {
                var orders = await _orderService.GetAllOrdersAsync();
                return Ok(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all orders");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Retrieves a specific order by its unique identifier including its order items
        /// </summary>
        /// <param name="id">The unique identifier of the order to retrieve</param>
        /// <returns>The requested order with its order items if found; otherwise returns NotFound</returns>
        
        // GET: api/orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderResponse>> GetOrder(Guid id)
        {
            try
            {
                var order = await _orderService.GetOrderByIdAsync(id);

                if (order == null)
                {
                    return Problem(detail: "This id NotFound", statusCode: 404, title: "Getting Order By Id");
                }

                return Ok(order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting order with ID {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Creates a new order with the provided order data
        /// </summary>
        /// <param name="orderDto">The order data to create</param>
        /// <returns>The newly created order with its generated identifier</returns>

        // POST: api/orders
        [HttpPost]
        public async Task<ActionResult<OrderResponse>> PostOrder([FromBody] OrderAddRequest orderDto)
        {
            try
            {
                var createdOrder = await _orderService.AddOrderAsync(orderDto);

                return CreatedAtAction(nameof(GetOrder),
                    new { id = createdOrder.OrderId },
                    createdOrder);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating new order");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Updates an existing order with the provided data
        /// </summary>
        /// <param name="id">The unique identifier of the order to update</param>
        /// <param name="orderDto">The updated order data</param>
        /// <returns>NoContent if successful; BadRequest if ID mismatch; NotFound if order doesn't exist</returns>

        // PUT: api/orders/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(Guid id, [FromBody] OrderUpdateRequest orderDto)
        {
            try
            {
                if (id != orderDto.OrderId)
                    return BadRequest("ID mismatch");

                var result = await _orderService.UpdateOrderAsync(orderDto);

                if (result == null)
                    return NotFound($"Order with ID {orderDto.OrderId} not found");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating order with ID {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Deletes an order with the specified unique identifier
        /// </summary>
        /// <param name="id">The unique identifier of the order to delete</param>
        /// <returns>NoContent if successful; NotFound if order doesn't exist</returns>

        // DELETE: api/orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(Guid? id)
        {
            try
            {
                var result = await _orderService.DeleteOrderAsync(id);
                if (!result)
                    return Problem(detail: "This id NotFound", statusCode: 404, title: "Deleteing Order" +" By Id");
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting order with ID {id}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}