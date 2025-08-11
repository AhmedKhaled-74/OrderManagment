namespace OrderManagement.Core.DTOs.OrderItemDTO
{
    public class OrderItemResponse
    {
        public Guid OrderItemId { get; set; }
        public Guid OrderId { get; set; }
        public Guid ItemId { get; set; }  
        public string? ItemName { get; set; } 
        public uint Quantity { get; set; }
        public decimal? UnitPrice { get; set; }

        public decimal? TotalPrice;
    }
}
