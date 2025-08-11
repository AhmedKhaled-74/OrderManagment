using OrderManagement.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Core.DTOs.ItemDTO
{
    public class ItemAddRequest
    {

        [Required]
        [StringLength(100, ErrorMessage = "Item name cannot exceed 100 characters.")]
        public string ItemName { get; set; } = string.Empty; // Default to empty string to avoid null reference issues
        [Required]
        [Range(typeof(decimal), "0.01", "79228162514264337593543950335", ErrorMessage = "Item price must be a positive number")]
        public decimal ItemPrice { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public uint? ItemTotalQuantity { get; set; }

        public Item ToItem()
        {
            return new Item
            {
                ItemName = ItemName,
                ItemPrice = ItemPrice,
                ItemTotalQuantity = ItemTotalQuantity,
            };
        }
    }
}
