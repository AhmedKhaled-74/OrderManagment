using OrderManagement.Core.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Core.Domain.Entities
{
    public class Address
    {
        public Guid AddressId { get; set; }
        public string Street { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public bool IsDefault { get; set; } = false; 
        public Guid UserId { get; set; } // Foreign key to the user
        public virtual AppUser User { get; set; } = null!; // Navigation property to the user


    }
}
