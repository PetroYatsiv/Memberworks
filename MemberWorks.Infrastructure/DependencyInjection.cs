using Microsoft.Extensions.DependencyInjection;
using MemberWorks.Application.Common.Interfaces;
using MemberWorks.Infrastructure.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MemberWorks.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<GoogleAuthOptions>(config.GetSection(GoogleAuthOptions.SectionName));
        services.Configure<JwtOptions>(config.GetSection(JwtOptions.SectionName));

        services.AddScoped<IGoogleTokenValidator, GoogleTokenValidator>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();

        return services;
    }
}