namespace SocialHub.Api.DTOs.Auth;

public record UserResponse(
    Guid Id,
    string Username,
    string Email,
    DateTimeOffset CreatedAt
);