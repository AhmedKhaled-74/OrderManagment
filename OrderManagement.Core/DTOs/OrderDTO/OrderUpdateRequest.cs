using OrderManagement.Core.Domain.Entities;

namespace OrderManagement.Core.DTOs.OrderDTO
{
    public class OrderUpdateRequest
    {
        public Guid OrderId { get; set; }
        public string? CustomerName { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; } = [];
        public Order ToOrder(Order existingOrder)
        {
            // Update only the fields that should be updatable
            existingOrder.CustomerName = CustomerName ?? existingOrder.CustomerName;

            if (OrderItems != null)
            {
                existingOrder.OrderItems = OrderItems;
            }

            return existingOrder;
        }
    }
}
