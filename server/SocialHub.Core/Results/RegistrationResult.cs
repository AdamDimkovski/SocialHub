namespace SocialHub.Core.Results;

public enum RegistrationStatus { Success, DuplicateUsername, DuplicateEmail }

public sealed class RegistrationResult
{
    public RegistrationStatus Status { get; }
    public Guid? UserId { get; }
    public bool IsSuccess => Status == RegistrationStatus.Success;

    private RegistrationResult(RegistrationStatus status, Guid? userId = null)
    {
        UserId = userId;
        Status = status;
    }

    public static RegistrationResult Success(Guid userId) => new(RegistrationStatus.Success, userId);
    public static RegistrationResult DuplicateUsername() => new(RegistrationStatus.DuplicateUsername);
    public static RegistrationResult DuplicateEmail() => new(RegistrationStatus.DuplicateEmail);
}