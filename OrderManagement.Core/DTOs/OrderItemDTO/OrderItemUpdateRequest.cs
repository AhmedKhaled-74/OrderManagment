using OrderManagement.Core.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace OrderManagement.Core.DTOs.OrderItemDTO
{
    public class OrderItemUpdateRequest
    {
        [Required]
        public Guid OrderItemId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public uint Quantity { get; set; }

        [Required]
        public decimal UnitPrice { get; set; }


        public OrderItem ToOrderItem()
        {
            return new OrderItem
            {
                OrderItemId = OrderItemId,
                Quantity = Quantity,
                UnitPrice = UnitPrice, // Default to 0 if null
            };
        }
    }
}
