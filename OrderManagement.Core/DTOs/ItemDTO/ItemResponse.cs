using OrderManagement.Core.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace OrderManagement.Core.DTOs.ItemDTO
{
    public class ItemResponse
    {
        public Guid ItemId { get; set; }
        public string? ItemName { get; set; } = string.Empty; // Default to empty string to avoid null reference issues
        public decimal? ItemPrice { get; set; }

        public uint? ItemTotalQuantity { get; set; }
    }
}
