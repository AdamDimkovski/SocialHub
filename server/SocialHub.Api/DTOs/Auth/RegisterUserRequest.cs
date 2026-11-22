using System.ComponentModel.DataAnnotations;

namespace SocialHub.Api.DTOs.Auth;

public sealed record RegisterUserRequest(
    string Username,
    string Email,
    string Password
); 