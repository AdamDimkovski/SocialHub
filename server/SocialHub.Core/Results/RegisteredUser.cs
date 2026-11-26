namespace SocialHub.Core.Results;
public sealed record RegisteredUser(Guid Id, string Username, string Email, DateTimeOffset CreatedAt);