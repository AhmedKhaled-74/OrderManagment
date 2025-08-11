using OrderManagement.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Core.DTOs.OrderItemDTO
{
    public class OrderItemAddRequest
    {
        [Required]
        public Guid OrderId { get; set; }
        [Required]
        public Guid ItemId { get; set; }  // Navigation property to the Order entity

        [Required]
        [Range(1, int.MaxValue)]
        public uint Quantity { get; set; } = 1;

        public OrderItem ToOrderItem()
        {
            return new OrderItem
            {
                OrderId = OrderId,
                ItemId = ItemId,
                Quantity = Quantity,
            };
        }
    }
}
