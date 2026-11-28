using SocialHub.Api.DTOs.Auth;
using SocialHub.Core.Abstractions;
using SocialHub.Core.Results;
using FluentValidation;

namespace SocialHub.Api.Endpoints;

public static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var auth = endpoints.MapGroup("/auth");

        auth.MapPost("/register", RegisterAsync)
            .WithName("Register")
            .WithSummary("Registers a new user.");

        auth.MapPost("/login", LoginAsync)
            .WithName("Login")
            .WithSummary("Authenticates a existing user.");

        return endpoints;
    }

    private static async Task<IResult> RegisterAsync(
    RegisterUserRequest request,
    IValidator<RegisterUserRequest> validator,
    IRegistrationService registrationService)
    {

        // Validation obj, waits for request, checks if valid, returns error in message.
        var validation = await validator.ValidateAsync(request);

        if (!validation.IsValid)
        {
            return Results.ValidationProblem(validation.ToDictionary());
        }

        var result = await registrationService.RegisterAsync(
            request.Username, request.Email, request.Password);

        return result.Status switch
        {
            RegistrationStatus.Success => Results.Created($"/api/users/{result.User!.Id}", new RegisterUserResponse(
                result.User.Id, result.User.Username, result.User.Email, result.User.CreatedAt)),
            RegistrationStatus.DuplicateUsername => Results.Conflict(new { error = "Username already taken." }),
            RegistrationStatus.DuplicateEmail => Results.Conflict(new { error = "Email already taken." }),
            _ => throw new InvalidOperationException($"Unexpected error has occured: {result.Status}")

        };
    }

    private static async Task<IResult> LoginAsync(
        LoginRequest request,
        IValidator<LoginRequest> validator,
        ILoginService loginService)
    {
        // Validation obj, waits for request, checks if valid, returns error in message.
        var validation = await validator.ValidateAsync(request);

        if (!validation.IsValid)
        {
            return Results.ValidationProblem(validation.ToDictionary());
        }

        var result = await loginService.LoginAsync(
            request.Identifier, request.Password);

        return result.Status switch
        {
            LoginStatus.Success => Results.Ok(new LoginResponse(result.User!.Id, result.User.Username)),
            LoginStatus.InvalidCredentials => Results.Problem(statusCode: 401, title: "Invalid credentials."),
            _ => throw new InvalidOperationException($"Unexpected error has occured: {result.Status}")
        };


    }
}

