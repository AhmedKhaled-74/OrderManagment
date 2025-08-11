using Microsoft.AspNetCore.Identity;
using OrderManagement.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Core.Domain.Identity
{
    public class AppUser : IdentityUser<Guid>
    {
        public string FullName { get; set; } = string.Empty;
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiration { get; set; }

        public DateTime? DateOfBirth { get; set; }
        public string? Region { get; set; } 
        public string ActiveStatus { get; set; } = "Active"; // Default to "Active" status
        public string? ProfilePictureUrl { get; set; } // URL to the user's profile picture
        public virtual Guid? AppAdminId { get; set; } = null!; // Navigation property to other users
        public virtual AppUser? AppAdmin { get; set; } = null!; // Navigation property to other users
        public virtual ICollection<AppUser> AppUsers { get; set; } = new List<AppUser>(); // Navigation property to other users

        public virtual ICollection<AppRole> Roles { get; set; } = new List<AppRole>(); // Navigation property to roles
        public virtual ICollection<Address>? Addresses { get; set; } = new List<Address>(); // Navigation property to addresses

    }
}
