using SocialHub.Core.Abstractions;
using SocialHub.Core.Results;
using Microsoft.EntityFrameworkCore;
using SocialHub.Infrastructure.Data;
using Npgsql;
using SocialHub.Core.Entities;
using SocialHub.Core.Constants;

namespace SocialHub.Infrastructure.Services;

public sealed class RegistrationService : IRegistrationService
{
    private readonly AppDbContext _db;
    private readonly IPasswordHasher _passwordhasher;
    public RegistrationService(AppDbContext dbContext, IPasswordHasher passwordHasher)
    {
        _db = dbContext;
        _passwordhasher = passwordHasher;
    }

    public async Task<RegistrationResult> RegisterAsync(string username, string email, string password)
    {   
        // Normalise details
        string normalisedUsername = username.Trim();
        string normalisedEmail = email.Trim().ToLowerInvariant();

        // Checks username for duplicate users
        if (await _db.Users.AnyAsync(u => u.Username == normalisedUsername))
            return RegistrationResult.DuplicateUsername();

        // Checks email for duplicate emails
        if (await _db.Users.AnyAsync(u => u.Email == normalisedEmail))
            return RegistrationResult.DuplicateEmail();

        // Creating new user
        var user = new User
        {
            Username = normalisedUsername,
            Email = normalisedEmail,
            PasswordHash = _passwordhasher.Hash(password)
        };

        _db.Users.Add(user);

        // Try catch handles UniqueViolation errors thrown by postgre
        try {
            await _db.SaveChangesAsync();
        }
        catch(DbUpdateException ex)
            when (ex.InnerException is PostgresException {SqlState: PostgresErrorCodes.UniqueViolation} pg)
        {
            if (pg.ConstraintName == UserConstraints.DatabaseEmail)
                return RegistrationResult.DuplicateEmail();

            if (pg.ConstraintName == UserConstraints.DatabaseUsername)
                return RegistrationResult.DuplicateUsername();

            throw;
        }

        // Returns sucessful only if theres no issues with try catch.
        return RegistrationResult.Success(user.Id);

        
    }
}