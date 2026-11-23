using SocialHub.Core.Abstractions;
using Microsoft.Extensions.Options;

namespace SocialHub.Infrastructure.Security;

public sealed class BcryptPasswordHasher : IPasswordHasher
{
    private readonly int _workFactor;

    public BcryptPasswordHasher(IOptions<PasswordHasherOptions> options)
    {
        _workFactor = options.Value.WorkFactor;
    }
    public string Hash(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password, _workFactor);
    }
    public bool Verify(string password, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }
    public bool NeedsRehash(string hash)
    {
        return BCrypt.Net.BCrypt.PasswordNeedsRehash(hash, _workFactor);
    }    
}