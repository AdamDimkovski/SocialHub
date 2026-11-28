using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using SocialHub.Core.Abstractions;
using SocialHub.Infrastructure.Data;
using SocialHub.Infrastructure.Security;
using SocialHub.Infrastructure.Services;

namespace SocialHub.Infrastructure;


public static class DependencyInjection
{

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {

        // Retrieve Default connection string
        var connectionString = configuration.GetConnectionString("Default")
            ?? throw new InvalidOperationException("Connection string 'Default' not found.");

        // Database connection (DbContext)
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddOptions<PasswordHasherOptions>()
            .Bind(configuration.GetSection(PasswordHasherOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        // Singleton Password Hasher
        services.AddSingleton<IPasswordHasher, BcryptPasswordHasher>();

        // Scoped Registration Service
        services.AddScoped<IRegistrationService, RegistrationService>();

        // Scoped Login Service
        services.AddScoped<ILoginService, LoginService>();

        // Singleton Dummy Hash Provider
        services.AddSingleton<DummyHashProvider>();

        return services;
    }

}