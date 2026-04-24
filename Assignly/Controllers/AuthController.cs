using System.Security.Claims;
using System.Threading.Tasks;
using Assignly.Core.DTOs.AuthDTOs;
using Assignly.Service.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Assignly.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync(RegisterRequest request)
        {
            var response = await _authService.RegisterAsync(request);
            if (response.IsFailure)
            {
                return StatusCode(response.StatusCode, response.Error);
            }
            return StatusCode(response.StatusCode, response.Data);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync(LoginRequest request)
        {
            var response = await _authService.LoginAsync(request);
            if (response.IsFailure)
            {
                return StatusCode(response.StatusCode, response.Error);
            }
            return StatusCode(response.StatusCode, response.Data);
        }

        [HttpGet("auth/login")]
        public IActionResult Login()
        {
            var redirectUrl = Url.Action("GoogleResponse");
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };

            return Challenge(properties, "Google");
        }

        [HttpGet("auth/google-response")]
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(
                CookieAuthenticationDefaults.AuthenticationScheme
            );

            if (!result.Succeeded)
                return BadRequest("Authentication failed");

            var claims = result.Principal.Identities.FirstOrDefault()?.Claims;

            var email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var name = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            var googleId = claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            // 🔹 Here you:
            // 1. Check if user exists
            // 2. Create if not
            // 3. Store in DB

            return Redirect("/weatherforecast?page=1&pageSize=5");
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmailAsync([FromQuery] string token)
        {
            var respone = await _authService.ConfirmEmailAsync(token);
            return StatusCode(respone.StatusCode, respone);
        }

        [HttpGet("forget-password")]
        public async Task<IActionResult> ForgetPasswordAsync([FromQuery] string email)
        {
            var respone = await _authService.ForgetPasswordAsync(email);
            return StatusCode(respone.StatusCode, respone);
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPasswordAsync(
            [FromQuery] string token,
            [FromBody] string newPassword
        )
        {
            var respone = await _authService.ResetPasswordAsync(token, newPassword);
            return StatusCode(respone.StatusCode, respone);
        }
    }
}
