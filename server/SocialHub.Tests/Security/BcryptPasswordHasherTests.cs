using Xunit;
using Microsoft.Extensions.Options;
using SocialHub.Infrastructure.Security;

namespace SocialHub.Tests.Security;

public class BcryptPasswordHasherTests
{
    private static BcryptPasswordHasher CreateHasher(int workFactor = 4)
    {
        var options = Options.Create(new PasswordHasherOptions
        {
            WorkFactor = workFactor
        });

        return new BcryptPasswordHasher(options);
    }
    [Fact]
    public void Hash_ReturnsBcryptFormattedString()
    {
        // Arrange
        var password = "MySecretPassword123!";
        var hasher = CreateHasher();

        // Act
        string hashedPassword = hasher.Hash(password);

        //Assert
        Assert.StartsWith("$2", hashedPassword);
        Assert.Equal(60, hashedPassword.Length);
    }
    [Fact]
    public void Hash_SamePasswordTwice_ReturnsDifferentHashes()
    {
        // Arrange
        var password = "MySecretPassword123!";
        var hasher = CreateHasher();

        // Act
        string hashedPassword1 = hasher.Hash(password);
        string hashedPassword2 = hasher.Hash(password);

        //Assert
        Assert.NotEqual(hashedPassword1, hashedPassword2);
    }
    [Fact]
    public void Verify_CorrectPassword_ReturnsTrue()
    {
        // Arrange
        var password = "MySecretPassword123!";
        var hasher = CreateHasher();

        // Act
        string hashedPassword = hasher.Hash(password);
        bool isValid = hasher.Verify(password, hashedPassword);

        //Assert
        Assert.True(isValid, "The verify call on Verify_CorrectPassword_ReturnsTrue is incorrect.");
    }
    [Fact]
    public void Verify_WrongPassword_ReturnsFalse()
    {
        // Arrange
        var password = "MySecretPassword123!";
        var attemptedPassword = "NoodleSoup558!";
        var hasher = CreateHasher();

        // Act
        string hashedPassword = hasher.Hash(password);
        bool notValid = hasher.Verify(attemptedPassword, hashedPassword);

        //Assert
        Assert.False(notValid, "The verify call on Verify_WrongPassword_ReturnsFalse is incorrect.");
    }
    [Fact]
    public void NeedsRehash_HashMadeWithLowerCost_ReturnsTrue()
    {
        // Arrange
        var password = "MySecretPassword123!";
        var oldHasher = CreateHasher(4);
        var currentHasher = CreateHasher(5);

        // Act
        string hashedPassword = oldHasher.Hash(password);
        bool needsRehash = currentHasher.NeedsRehash(hashedPassword);

        //Assert
        Assert.True(needsRehash, "The NeedsRehash call on NeedsRehash_HashMadeWithLowerCost_ReturnsTrue is incorrect.");
    }
    [Fact]
    public void NeedsRehash_HashMadeWithCurrentCost_ReturnsFalse()
    {
        // Arrange
        var password = "MySecretPassword123!";
        var currentHasher = CreateHasher(4);

        // Act
        string hashedPassword = currentHasher.Hash(password);
        bool needsRehash = currentHasher.NeedsRehash(hashedPassword);

        //Assert
        Assert.False(needsRehash, "The NeedsRehash call on NeedsRehash_HashMadeWithCurrentCost_ReturnsFalse is incorrect.");
    }
    

}