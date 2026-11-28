namespace SocialHub.Api.DTOs.Auth;

public sealed record LoginRequest(
    string Identifier,
    string Password
); 