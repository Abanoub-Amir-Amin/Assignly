using System;
using Assignly.Core.DTOs.AuthDTOs;
using Assignly.Core.DTOs.Results;
using Assignly.Data.Models;
using Assignly.Infrastructure.Repositories;
using Azure.Core;
using Microsoft.AspNetCore.Identity;

namespace Assignly.Service.Services;

public class AuthService(
    UserManager<User> userManager,
    ITokenService tokenService,
    IEmailService emailService
) : IAuthService
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly ITokenService _tokenService = tokenService;
    private readonly IEmailService _emailService = emailService;

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

        if (!isPasswordCorrect)
        {
            return Result<LoginResponse>.Failure("Invalid Email or password.", 401);
        }
        if (!user.EmailConfirmed)
        {
            return Result<LoginResponse>.Failure("Please confirm your email.", 401);
        }
        Dictionary<string, string> claims = GetClaimsFromUser(user);
        var token = _tokenService.GenerateToken(claims);
        return Result<LoginResponse>.Success(new LoginResponse { Token = token });
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
        var claims = GetClaimsFromUser(user);
        var token = _tokenService.GenerateToken(claims);

        await _emailService.SendEmailAsync(
            user.Email,
            "Welcome to Assignly, Please confirm your email",
            "<!DOCTYPE html><html><head><meta charset=\"UTF-8\"><title>Email Confirmation</title></head><body style=\"margin:0; padding:0; font-family:Arial, sans-serif; background-color:#f4f4f4;\"><table align=\"center\" width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" style=\"padding:20px;\"><tr><td align=\"center\"><table width=\"600\" cellpadding=\"0\" cellspacing=\"0\" style=\"background:#ffffff; border-radius:8px; padding:30px;\"><tr><td align=\"center\" style=\"padding-bottom:20px;\"><h2 style=\"margin:0; color:#333;\">Confirm Your Email</h2></td></tr><tr><td style=\"color:#555; font-size:16px; line-height:1.5;\"><p>Hello,</p><p>Thanks for signing up! Please confirm your email address by clicking the button below.</p></td></tr><tr><td align=\"center\" style=\"padding:30px 0;\"><a href=\"{{CONFIRMATION_LINK}}\" style=\"background-color:#007bff; color:#ffffff; padding:12px 25px; text-decoration:none; border-radius:5px; font-size:16px;\">Confirm Email</a></td></tr><tr><td style=\"color:#777; font-size:14px;\"><p>If the button doesn't work, copy and paste this link into your browser:</p><p style=\"word-break:break-all;\">{{CONFIRMATION_LINK}}</p></td></tr><tr><td style=\"padding-top:30px; font-size:12px; color:#aaa; text-align:center;\"><p>If you didn’t create an account, you can ignore this email.</p><p>&copy; 2026 Assignly</p></td></tr></table></td></tr></table></body></html>"
        );
        return Result<string>.Success("You have successfully been registered.");
    }

    private Dictionary<string, string> GetClaimsFromUser(User user)
    {
        return new Dictionary<string, string>
        {
            { "UserName", user.UserName },
            { "Email", user.Email },
            { "Role", user.Role.ToString() },
        };
    }
}
