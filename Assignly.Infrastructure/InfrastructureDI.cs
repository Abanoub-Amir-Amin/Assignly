using Assignly.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Assignly.Infrastructure;

public static class InfrastructureDI
{
    public static IServiceCollection AddInfrastructureDI(this IServiceCollection services)
    {
        services.AddScoped<IAttachmentRepository, AttachmentRepository>();

        return services;
    }
}
