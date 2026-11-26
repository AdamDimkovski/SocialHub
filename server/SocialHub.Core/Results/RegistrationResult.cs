namespace SocialHub.Core.Results;

public enum RegistrationStatus { Success, DuplicateUsername, DuplicateEmail }

public sealed class RegistrationResult
{
    public RegistrationStatus Status { get; }
    public RegisteredUser? User { get; }
    public bool IsSuccess => Status == RegistrationStatus.Success;

    private RegistrationResult(RegistrationStatus status, RegisteredUser? user = null)
    {
        User = user;
        Status = status;
    }

    public static RegistrationResult Success(RegisteredUser userId) => new(RegistrationStatus.Success, userId);

    public static RegistrationResult DuplicateUsername() => new(RegistrationStatus.DuplicateUsername);

    public static RegistrationResult DuplicateEmail() => new(RegistrationStatus.DuplicateEmail);
}