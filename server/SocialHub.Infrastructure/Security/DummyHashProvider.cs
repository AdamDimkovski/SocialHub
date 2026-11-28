using SocialHub.Core.Abstractions;

namespace SocialHub.Infrastructure.Security;

public sealed class DummyHashProvider 
{
    public string DummyHash { get; }

    public DummyHashProvider(IPasswordHasher passwordHasher)
    {
        DummyHash = passwordHasher.Hash("lololol6767420.");
    } 
}