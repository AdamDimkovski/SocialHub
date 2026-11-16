using System.ComponentModel.DataAnnotations;

namespace SocialHub.Api.DTOs.Auth;

public record RegisterRequest(
    [property: Required, MinLength(3), MaxLength(50)] string Username,
    [property: Required, EmailAddress, MinLength(3), MaxLength(100)] string Email,
    [property: Required] string Password
);