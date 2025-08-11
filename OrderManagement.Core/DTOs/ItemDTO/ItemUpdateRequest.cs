using OrderManagement.Core.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace OrderManagement.Core.DTOs.ItemDTO
{
    public class ItemUpdateRequest
    {
        [Required]
        public Guid ItemId { get; set; }

        [StringLength(100, ErrorMessage = "Item name cannot exceed 100 characters.")]
        public string? ItemName { get; set; }  // Default to empty string to avoid null reference issues
        [Range(typeof(decimal), "0.01", "79228162514264337593543950335", ErrorMessage = "Item price must be a positive number")]
        public decimal? ItemPrice { get; set; }

        [Range(0, int.MaxValue)]
        public uint? ItemTotalQuantity { get; set; }

        public Item ToItem()
        {
            return new Item
            {
                ItemId = ItemId,
                ItemName = ItemName,
                ItemPrice = ItemPrice,
                ItemTotalQuantity = ItemTotalQuantity
            };
        }
    }
}
