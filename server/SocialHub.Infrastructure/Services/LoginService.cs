using SocialHub.Core.Abstractions;
using SocialHub.Core.Results;
using Microsoft.EntityFrameworkCore;
using SocialHub.Infrastructure.Data;
using SocialHub.Core.Entities;

namespace SocialHub.Infrastructure.Services;

public sealed class LoginService : ILoginService
{
    private readonly AppDbContext _db;
    private readonly IPasswordHasher _passwordHasher;
    public LoginService(AppDbContext dbContext, IPasswordHasher passwordHasher)
    {
        _db = dbContext;
        _passwordHasher = passwordHasher;
    }
    public async Task<LoginResult> LoginAsync(string identifier, string password)
    {
        // Normalise details
        string normalisedIdentifier = identifier.Trim();
        bool isEmail = normalisedIdentifier.Contains('@');
        if(isEmail) normalisedIdentifier = normalisedIdentifier.ToLowerInvariant();



        
        var authenticatedUser = new AuthenticatedUser(user.Id, user.Username, user.Email, user.CreatedAt);
        
        // Returns sucessful only if theres no issues with try catch.
        return LoginResult.Success(authenticatedUser);
    }

}