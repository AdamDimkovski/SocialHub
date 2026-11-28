using SocialHub.Core.Results;

namespace SocialHub.Core.Abstractions;

public interface ILoginService
{
    Task<LoginResult> LoginAsync(string identifier, string password);
}