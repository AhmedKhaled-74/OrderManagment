using OrderManagement.Core.DTOs.OrderDTO;
using OrderManagement.Core.DTOs.OrderItemDTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Core.Domain.Entities
{
    public class Order
    {
        [Key]
        public Guid OrderId { get; set; }  
        public string? OrderNumber { get; set; }
        [Required]
        public DateTime OrderDate { get; set; } = DateTime.Now;
        [Required]
        [StringLength(50, ErrorMessage = "Customer name cannot exceed 50 characters.")]
        public string? CustomerName { get; set; }

        public decimal TotalAmount => OrderItems?.Sum(item => item.TotalPrice) ?? 0;

        public ICollection<OrderItem> OrderItems { get; set; } = [];

        public OrderResponse ToOrderResponse()
        {
            return new OrderResponse
            {
                OrderId = this.OrderId,
                OrderNumber = this.OrderNumber,
                OrderDate = this.OrderDate,
                CustomerName = this.CustomerName,
                TotalAmount = this.TotalAmount,
                OrderItems = OrderItems?
                    .Select(item => item.ToOrderItemResponse())
                    .ToList() ?? new List<OrderItemResponse>()
            };
        }
    }
}
