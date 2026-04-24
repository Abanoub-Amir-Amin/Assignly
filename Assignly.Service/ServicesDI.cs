using Assignly.Data.Models;
using Assignly.Service.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Assignly.Service;

public static class ServicesDI
{
    public static IServiceCollection AddServicesDI(this IServiceCollection services)
    {
        services.AddScoped<IAttachmentService, AttachmentService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddSingleton<ITokenService, TokenService>();
        services.AddSingleton<IEmailService, EmailService>();
        services.AddSingleton<PasswordHasher<User>>();

        return services;
    }
}
