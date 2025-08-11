using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Core.DTOs.UserDTO
{
    public class AuthenticationResponse
    {
        public string? PersonName { get; set; }
        public string? UserName { get; set; } 
        public string? Email { get; set; }
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpirationDate { get; set; }
        public DateTime Expiration { get; set; }

    }
}
