namespace SocialHub.Core.Results;

public enum LoginStatus { Success, InvalidCredentials }

public sealed class LoginResult
{
    public LoginStatus Status { get; }
    public AuthenticatedUser? User { get; }
    public bool IsSuccess => Status == LoginStatus.Success;

    private LoginResult(LoginStatus status, AuthenticatedUser? user = null)
    {
        User = user;
        Status = status;
    }
    public static LoginResult Success(AuthenticatedUser authenticatedUser) => new(LoginStatus.Success, authenticatedUser);

    public static LoginResult InvalidCredentials() => new(LoginStatus.InvalidCredentials);


}