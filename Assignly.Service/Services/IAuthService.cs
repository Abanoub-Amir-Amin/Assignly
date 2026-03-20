using Assignly.Core.DTOs.AuthDTOs;
using Assignly.Core.DTOs.Results;

namespace Assignly.Service.Services;

public interface IAuthService
{
    public Task<Result<LoginResponse>> LoginAsync(LoginRequest request);
    public Task<Result<string>> RegisterAsync(RegisterRequest request);
}
