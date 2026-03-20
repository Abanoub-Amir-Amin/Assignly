using Assignly.Core.DTOs.AuthDTOs;
using Assignly.Core.DTOs.Results;
using Assignly.Data.Models;
using Assignly.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;

namespace Assignly.Service.Services;

public class AuthService(UserManager<User> userManager, ITokenService tokenService) : IAuthService
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly ITokenService _tokenService = tokenService;

    public async Task<Result<LoginResponse>> LoginAsync(LoginRequest request)
    {
        if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
        {
            return Result<LoginResponse>.Failure("Email and password are required.", 400);
        }
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            return Result<LoginResponse>.Failure("Invalid Email or password.", 401);
        }
        var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, request.Password);
        Dictionary<string, string> claims = new()
        {
            { "UserName", user.UserName },
            { "Email", user.Email },
            { "Role", user.Role.ToString() },
        };
        if (isPasswordCorrect)
        {
            var token = _tokenService.GenerateToken(claims);
            return Result<LoginResponse>.Success(new LoginResponse { Token = token });
        }
        return Result<LoginResponse>.Failure("Invalid Email or password.", 401);
    }

    public async Task<Result<string>> RegisterAsync(RegisterRequest request)
    {
        if (string.IsNullOrEmpty(request.UserName))
            return Result<string>.Failure("User name is required.", 400);
        if (string.IsNullOrEmpty(request.Password))
            return Result<string>.Failure("Password is required.", 400);
        if (string.IsNullOrEmpty(request.Email))
            return Result<string>.Failure("Email is required.", 400);
        var hasher = new PasswordHasher<User>();
        var user = new User
        {
            UserName = request.UserName,
            PasswordHash = hasher.HashPassword(new User(), request.Password),
            Email = request.Email,
            Role = request.Role,
        };
        //await _userManager.AddToRoleAsync(user, request.Role.ToString());
        var registerationResult = await _userManager.CreateAsync(user);
        if (!registerationResult.Succeeded)
        {
            var errors = registerationResult.Errors.ToList();
            return Result<string>.Failure(errors[0].Description, 500);
        }
        return Result<string>.Success("You have successfully been registered.");
    }
}
