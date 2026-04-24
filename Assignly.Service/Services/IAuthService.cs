using Assignly.Core.DTOs.AuthDTOs;
using Assignly.Core.DTOs.Results;

namespace Assignly.Service.Services;

public interface IAuthService
{
    public Task<Result<LoginResponse>> LoginAsync(LoginRequest request);
    public Task<Result<string>> RegisterAsync(RegisterRequest request);
    public Task<Result<string>> ConfirmEmailAsync(string token);
    public Task<Result<string>> ForgetPasswordAsync(string email);
    public Task<Result<string>> ResetPasswordAsync(string token, string newPassword);
}
