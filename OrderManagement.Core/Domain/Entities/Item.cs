using OrderManagement.Core.DTOs.ItemDTO;
using System.ComponentModel.DataAnnotations;

namespace OrderManagement.Core.Domain.Entities
{
    public class Item
    {
        [Key]
        public Guid ItemId { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Item name cannot exceed 100 characters.")]
        public string? ItemName { get; set; } = string.Empty; // Default to empty string to avoid null reference issues
        [Required]
        [Range(typeof(decimal), "0.01", "79228162514264337593543950335", ErrorMessage = "Item price must be a positive number")]
        public decimal? ItemPrice { get; set; }

        [Range(0, int.MaxValue)]
        public uint? ItemTotalQuantity { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; } = []; // Navigation property to the OrderItem entity

        public ItemResponse ToItemResponse()
        {
            return new ItemResponse
            {
                ItemId = this.ItemId,
                ItemName = this.ItemName,
                ItemPrice = this.ItemPrice,
                ItemTotalQuantity = this.ItemTotalQuantity
            };

        }
    }
}