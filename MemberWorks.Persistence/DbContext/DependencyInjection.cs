using MemberWorks.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MemberWorks.Persistence.Migrations;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration config)
    {
        var provider = config["Database:Provider"] ?? "Postgres";
        var connectionString = config.GetConnectionString("Default")
                               ?? throw new InvalidOperationException("Connection string 'Default' is not configured.");

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            switch (provider.ToLowerInvariant())
            {
                case "postgres":
                    options.UseNpgsql(connectionString,
                        b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));
                    break;
                
                default:
                    throw new NotSupportedException($"Database provider '{provider}' is not supported.");
            }
        });

        services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());

        return services;
    }
}