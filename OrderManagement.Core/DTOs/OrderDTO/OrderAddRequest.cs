using OrderManagement.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Core.DTOs.OrderDTO
{
    public class OrderAddRequest
    {
        [Required]
        [StringLength(50, ErrorMessage = "Customer name cannot exceed 50 characters.")]
        public string? CustomerName { get; set; }
   

       // public ICollection<OrderItem> OrderItems { get; set; } = [];

        public Order ToOrder()
        {
          return new Order
          {
              CustomerName = this.CustomerName,

          };
        }

    }
}
