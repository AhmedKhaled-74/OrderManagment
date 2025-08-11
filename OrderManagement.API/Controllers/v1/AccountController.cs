using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using OrderManagement.Core.Domain.Identity;
using OrderManagement.Core.DTOs.UserDTO;
using OrderManagement.Core.ServiceContracts;
using System.Security.Claims;

namespace OrderManagement.API.Controllers.v1
{
    /// <summary>
    /// For managing Accounts in the system.
    /// </summary>
    [AllowAnonymous]
    [ApiVersion("1.0")]
    public class AccountController : CustomHelperController
    {
        private readonly ILogger<AccountController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IJwtService _jwtService;

        /// <summary>
        /// constructor for AccountController
        /// </summary>
        public AccountController(ILogger<AccountController>logger , 
            UserManager<AppUser> userManager , 
            RoleManager<AppRole> roleManager , 
            SignInManager<AppUser> signInManager ,
            IJwtService jwtService
            ) 
        {
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
        }

        /// <summary>
        /// Register to the app
        /// </summary>
        /// <param name="registerDTO">valid register feilds</param>
        /// <returns>The newly created user</returns>
        [HttpPost]
        [Route("register")]
        public async Task<ActionResult<AppUser>> PostRegister(RegisterDTO registerDTO)
        {
            try
            {
                AppUser appUser = registerDTO.ToAppUser();
                var result = await _userManager.CreateAsync(appUser, registerDTO.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(appUser, isPersistent: false);
                    var authenticationResponse = _jwtService.CreateJwtToken(appUser);
                    appUser.RefreshToken = authenticationResponse.RefreshToken;
                    appUser.RefreshTokenExpiration = authenticationResponse.Expiration;

                    await _userManager.UpdateAsync(appUser);
                    return Ok(authenticationResponse);
                }
                return Problem(string.Join("|",result.Errors.Select(e => e.Description)));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");

            }
        }
        /// <summary>
        /// Login
        /// </summary>
        /// <param name="loginDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<AppUser>> Login(LoginDTO loginDTO)
        {
            try
            {

                var result = await _signInManager.PasswordSignInAsync(loginDTO.Email!, loginDTO.Password!,loginDTO.IsPersistent,false);
                if (result.Succeeded)
                {

                    var user = await _userManager.FindByEmailAsync(loginDTO.Email!);
                    if (user == null)
                    {
                        return Problem("invalid userName");
                    }
                    var authenticationResponse = _jwtService.CreateJwtToken(user);
                    user.RefreshToken = authenticationResponse.RefreshToken;
                    user.RefreshTokenExpiration = authenticationResponse.RefreshTokenExpirationDate;
                    await _userManager.UpdateAsync(user);

                    return Ok(authenticationResponse);
                }
                return Problem("Invalid email or password");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");

            }
        }
        /// <summary>
        /// logout approach
        /// </summary>
        /// <returns></returns>
        [HttpGet]  
        [Route("logout")]
        public async Task<ActionResult> LogOut()
        {
            try
            {

                await _signInManager.SignOutAsync();
                return Ok("LogOut Done");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, "Internal server error");

            }
        }
        /// <summary>
        /// AccessDenied
        /// </summary> 
        /// <returns>Unauthorized-"Not Allowed"</returns>

        [HttpGet]
        [Route("AccessDenied")]

        public ActionResult AccessDenied()
        {
            return Unauthorized("Not Allowed");
        }

        /// <summary>
        /// Check Email Exists
        /// </summary>
        /// <param name="email">email</param>
        /// <returns>true or false</returns>

        [HttpGet]
        [Route("register/check-email")]

        public async Task<IActionResult> CheckEmailExists(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return Ok(false);
                
            }
            return Ok(true);
        }

        /// <summary>
        /// Generate New Token by refreshtoken
        /// </summary>
        /// <param name="token">exsist token</param>
        /// <returns>new auth instance</returns>
        [HttpPost]
        [Route("generate-new-token")]

        public async Task<IActionResult> GenerateNewToken(TokenModel token)
        {
            
            if (token == null)
            {
                return BadRequest("Invalid Client Request");
                
            }
            var principal = _jwtService.GetJwtPrincipal(token.Token);
            if (principal == null)
                return BadRequest("Invalid Jwt access token");

            var email = principal.FindFirstValue(ClaimTypes.Email);
            if (email == null)
                return BadRequest("Invalid Jwt access token no email claim");
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || user.RefreshToken != token.RefreshToken || user.RefreshTokenExpiration <= DateTime.UtcNow)
                return BadRequest("Invalid Jwt access refreshtoken");

            var authRes = _jwtService.CreateAccessTokenOnly(user, user.RefreshToken!, user.RefreshTokenExpiration);

            return Ok(authRes);
        }
    }
}
