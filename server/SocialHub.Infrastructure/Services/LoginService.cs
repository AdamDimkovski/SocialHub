using SocialHub.Core.Abstractions;
using SocialHub.Core.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SocialHub.Infrastructure.Data;
using SocialHub.Core.Entities;
using SocialHub.Infrastructure.Security;

namespace SocialHub.Infrastructure.Services;

public sealed class LoginService : ILoginService
{
    private readonly AppDbContext _db;
    private readonly IPasswordHasher _passwordHasher;
    private readonly DummyHashProvider _dummyHashProvider;
    private readonly ILogger<LoginService> _logger;
    public LoginService(AppDbContext dbContext, IPasswordHasher passwordHasher, DummyHashProvider dummyHash
    , ILogger<LoginService> logger)
    {
        _db = dbContext;
        _passwordHasher = passwordHasher;
        _dummyHashProvider = dummyHash;
        _logger = logger;
    }
    public async Task<LoginResult> LoginAsync(string identifier, string password)
    {
        // Normalise details.
        string normalisedIdentifier = identifier.Trim();
        bool isEmail = normalisedIdentifier.Contains('@');
        if (isEmail) normalisedIdentifier = normalisedIdentifier.ToLowerInvariant();

        // Query User identifer.
        User? user = await _db.Users.FirstOrDefaultAsync(u =>
            isEmail ? u.Email == normalisedIdentifier
                    : u.Username == normalisedIdentifier);

        // Choose which hash to verify against.
        string hashToCheck = user?.PasswordHash ?? _dummyHashProvider.DummyHash;

        // Run a single verify statement.
        bool passwordValid = _passwordHasher.Verify(password, hashToCheck);

        // Unknown user or wrong password is a identical result.
        if (user is null || !passwordValid)
            return LoginResult.InvalidCredentials();

        // Password correct -> upgrade the hash if the cost is out of date.
        if (_passwordHasher.NeedsRehash(user.PasswordHash))
        {
            try
            {
                user.PasswordHash = _passwordHasher.Hash(password);
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // The old hash still works, so the upgrade can happen on the next login.
                _logger.LogWarning(ex, "Password rehash could not be saved for user {UserId}.", user.Id);
            }
        }

        var authenticatedUser = new AuthenticatedUser(user.Id, user.Username, user.Email);

        return LoginResult.Success(authenticatedUser);
    }

}