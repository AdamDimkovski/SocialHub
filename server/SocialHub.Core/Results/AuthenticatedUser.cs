namespace SocialHub.Core.Results;
public sealed record AuthenticatedUser(Guid Id, string Username, string Email, DateTimeOffset CreatedAt);