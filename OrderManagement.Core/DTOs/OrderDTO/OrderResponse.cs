using OrderManagement.Core.Domain.Entities;
using OrderManagement.Core.DTOs.OrderItemDTO;

namespace OrderManagement.Core.DTOs.OrderDTO
{
    public class OrderResponse
    {
        public Guid OrderId { get; set; }
        public string? OrderNumber { get; set; }
        public DateTime? OrderDate { get; set; }
        public string? CustomerName { get; set; }

        public decimal TotalAmount { get; set; }
        public List<OrderItemResponse> OrderItems { get; set; } = new List<OrderItemResponse>();
    }
}
