using System.Security.Claims;
using FUNewsManagement_CoreAPI.BLL.DTOs.SystemAccount;
using FUNewsManagement_CoreAPI.BLL.Models;
using FUNewsManagement_CoreAPI.BLL.Services.Implements;
using FUNewsManagement_CoreAPI.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FUNewsManagement_CoreAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly ISystemAccountService _systemAccountService;
        private readonly IConfiguration _config;

        public AuthController(AuthService authService, IConfiguration config, ISystemAccountService systemAccountService)
        {
            _authService = authService;
            _config = config;
            _systemAccountService = systemAccountService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> DoAuth([FromBody] SystemAccountAuthDTO credentials)
        {
            if (credentials == null ||
                string.IsNullOrEmpty(credentials.AccountEmail) ||
                string.IsNullOrEmpty(credentials.AccountPassword))
            {
                return BadRequest(new APIResponse<string>
                {
                    StatusCode = 400,
                    Message = "Email or password cannot be empty."
                });
            }

            var adminAccount = _config.GetSection("AdminAccount");
            var adminEmail = adminAccount["Email"];
            var adminPassword = adminAccount["Password"];

            if (credentials.AccountEmail == adminEmail && credentials.AccountPassword == adminPassword)
            {
                var accessToken = _authService.GenerateAccessToken(adminEmail, "Admin", 9999);
                var refreshToken = _authService.GenerateRefreshToken();
                TokenStore.RefreshTokens[adminEmail] = refreshToken;

                return Ok(new APIResponse<AuthResponse>
                {
                    StatusCode = 200,
                    Message = "Admin authenticated.",
                    Data = new AuthResponse
                    {
                        AccessToken = accessToken,
                        RefreshToken = refreshToken
                    }
                });
            }

            // Normal user
            var authInfo = await _systemAccountService.GetAuth(credentials.AccountEmail);
            if (authInfo == null)
            {
                return Unauthorized(new APIResponse<string>
                {
                    StatusCode = 401,
                    Message = "Invalid email or password."
                });
            }

            var hashedPassword = _authService.ComputeSha256Hash(credentials.AccountPassword);
            if (!authInfo.AccountPassword.Equals(hashedPassword))
            {
                return Unauthorized(new APIResponse<string>
                {
                    StatusCode = 401,
                    Message = "Invalid email or password."
                });
            }

            var accountInfo = await _systemAccountService.GetByEmailAsync(credentials.AccountEmail);
            string role = accountInfo.AccountRole == 1 ? "Staff" : "Lecturer";

            var access = _authService.GenerateAccessToken(credentials.AccountEmail, role, accountInfo.AccountId);
            var refresh = _authService.GenerateRefreshToken();
            TokenStore.RefreshTokens[credentials.AccountEmail] = refresh;

            return Ok(new APIResponse<AuthResponse>
            {
                StatusCode = 200,
                Message = "Authentication successful.",
                Data = new AuthResponse
                {
                    AccessToken = access,
                    RefreshToken = refresh
                }
            });
        }

        [AllowAnonymous]
        [HttpPost("refresh")]
        public IActionResult Refresh([FromBody] RefreshRequest request)
        {
            if (request == null)
            {
                return BadRequest(new APIResponse<string> { StatusCode = 400, Message = "Invalid request" });
            }

            var principal = _authService.GetPrincipalFromExpiredToken(request.AccessToken);
            if (principal == null)
            {
                return BadRequest(new APIResponse<string> { StatusCode = 400, Message = "Invalid access token" });
            }

            var email = principal.FindFirstValue(ClaimTypes.Email);
            if (!TokenStore.RefreshTokens.TryGetValue(email, out var storedRefreshToken) ||
                storedRefreshToken != request.RefreshToken)
            {
                return Unauthorized(new APIResponse<string> { StatusCode = 401, Message = "Invalid refresh token" });
            }

            var userIdClaim = principal.FindFirstValue("UserId");
            var userId = short.TryParse(userIdClaim, out var id) ? id : (short)0;

            var role = principal.FindFirstValue(ClaimTypes.Role);
            var newAccessToken = _authService.GenerateAccessToken(email, role, userId);
            var newRefreshToken = _authService.GenerateRefreshToken();

            TokenStore.RefreshTokens[email] = newRefreshToken;

            return Ok(new APIResponse<AuthResponse>
            {
                StatusCode = 200,
                Message = "Token refreshed successfully.",
                Data = new AuthResponse
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken
                }
            });
        }

        [AllowAnonymous]
        [HttpPost("logout")]
        public IActionResult Logout([FromBody] string email)
        {
            TokenStore.RefreshTokens.Remove(email);
            return Ok(new APIResponse<string>
            {
                StatusCode = 200,
                Message = "Logged out successfully."
            });
        }

        [Authorize]
        [HttpGet("me")]
        public IActionResult GetMe()
        {
            if (!User.Identity?.IsAuthenticated ?? false)
            {
                return Unauthorized(new
                {
                    statusCode = 401,
                    message = "Unauthorized access."
                });
            }

            var email = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Email)?.Value;
            var role = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Role)?.Value;
            var userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

            return Ok(new
            {
                statusCode = 200,
                message = "User info retrieved successfully.",
                data = new
                {
                    userId,
                    email,
                    role
                }
            });
        }
    }
}
