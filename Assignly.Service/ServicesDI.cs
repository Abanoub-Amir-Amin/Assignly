using Assignly.Service.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Assignly.Service;

public static class ServicesDI
{
    public static IServiceCollection AddServicesDI(this IServiceCollection services)
    {
        services.AddScoped<IAttachmentService, AttachmentService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddSingleton<ITokenService, TokenService>();

        return services;
    }
}
