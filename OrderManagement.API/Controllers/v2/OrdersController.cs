using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Core.DTOs.OrderDTO;
using OrderManagement.Core.ServiceContracts;
using OrderManagement.Core.Services;

namespace OrderManagement.API.Controllers.v2
{
    /// <summary>
    /// For managing orders in the system.
    /// </summary>
    /// 
    [ApiVersion("2.0")]
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
        public async Task<ActionResult<IEnumerable<string>>> GetOrders()
        {
            try
            {
                var orders = await _orderService.GetAllOrdersAsync();
                return Ok(orders?.Select(o => new {o.OrderId,o.CustomerName}));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all orders");
                return StatusCode(500, "Internal server error");
            }
        }

    }
}