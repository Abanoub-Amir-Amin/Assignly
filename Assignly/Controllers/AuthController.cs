using System.Threading.Tasks;
using Assignly.Core.DTOs.AuthDTOs;
using Assignly.Service.Services;
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
    }
}
