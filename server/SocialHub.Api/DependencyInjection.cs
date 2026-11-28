using FluentValidation;
using SocialHub.Api.Validators;

namespace SocialHub.Api;

public static class DependencyInjection
{

    public static IServiceCollection AddApi(this IServiceCollection services)
    {
        services.Configure<RouteHandlerOptions>(options =>
        {
            options.ThrowOnBadRequest = false;
        });

        // Automatically registers all validators in the same assembly as Program
        services.AddValidatorsFromAssemblyContaining<RegisterUserRequestValidator>();

        services.AddProblemDetails();

        // Add services to the container.
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        services.AddOpenApi();


        return services;
    }

}