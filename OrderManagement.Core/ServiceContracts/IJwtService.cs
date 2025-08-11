using OrderManagement.Core.Domain.Identity;
using OrderManagement.Core.DTOs.UserDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Core.ServiceContracts
{
    public interface IJwtService
    {
        AuthenticationResponse CreateJwtToken(AppUser user);
        ClaimsPrincipal? GetJwtPrincipal(string? token);
        public AuthenticationResponse CreateAccessTokenOnly(AppUser user, string existingRefreshToken, DateTime? existingRefreshExpiry);
    }
}
