using OrderManagement.Core.DTOs.OrderItemDTO;
using System;
using System.ComponentModel.DataAnnotations;

namespace OrderManagement.Core.Domain.Entities
{
    public class OrderItem
    {
        [Key]
        public Guid OrderItemId { get; set; }
        [Required]
        public Guid OrderId { get; set; } // Navigation property to the Order entity
        [Required]
        public Guid ItemId { get; set; }  // Navigation property to the Item entity
        [Required]
        public string? ItemName { get; set; } // Navigation property to the Item entity
        [Required]
        [Range(1, int.MaxValue)]
        public uint Quantity { get; set; }
        [Required]
        [Range(typeof(decimal), "0.01", "79228162514264337593543950335", ErrorMessage = "Unit price must be a positive number")]
        public decimal? UnitPrice { get; set; } 
        public decimal? TotalPrice => Quantity * UnitPrice;
        public Order? Order { get; set; }  // Navigation property to the Order entity
        public Item? Item { get; set; }  // Navigation property to the Item entity
        public OrderItemResponse ToOrderItemResponse()
        {
            return new OrderItemResponse
            {
                OrderItemId = this.OrderItemId,
                OrderId = this.OrderId,
                ItemId = this.ItemId,
                ItemName = this.ItemName,
                Quantity = this.Quantity,
                UnitPrice = this.UnitPrice,
                TotalPrice = this.TotalPrice,                
            };
        }
    }
}
