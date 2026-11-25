using SocialHub.Core.Results;

namespace SocialHub.Core.Abstractions;

public interface IRegistrationService
{
    Task<RegistrationResult> RegisterAsync(string username, string email, string password);
}