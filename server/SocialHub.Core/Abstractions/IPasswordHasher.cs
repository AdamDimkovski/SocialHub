namespace SocialHub.Core.Abstractions;

public interface IPasswordHasher
{
    string Hash(string password); // Used at register.
    bool Verify(string password, string hash); // Used at Login.
    bool NeedsRehash(string hash); // Used at Login.
}