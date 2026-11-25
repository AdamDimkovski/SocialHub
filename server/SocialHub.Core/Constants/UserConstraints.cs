namespace SocialHub.Core.Constants;

public static class UserConstraints
{
    public const int UsernameMinLength = 3;
    public const int UsernameMaxLength = 50;
    public const int EmailMinLength = 3;
    public const int EmailMaxLength = 100;
    public const int PasswordMinLength = 8;
    public const int PasswordMaxLength = 50;
    public const string DatabaseUsername = "IX_users_username";
    public const string DatabaseEmail = "IX_users_email";
}