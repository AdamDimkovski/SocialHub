using FluentValidation.TestHelper;
using SocialHub.Api.DTOs.Auth;
using SocialHub.Api.Validators;
using SocialHub.Core.Constants;

namespace SocialHub.Tests.Validators;

public class RegisterUserRequestValidatorTests
{
    private readonly RegisterUserRequestValidator _validator = new();

    private static RegisterUserRequest ValidRequest() =>
        new("Johnpork", "example@email.com", "Password123");

    // Invalid Usernames:
    [Theory]
    [InlineData("")]    // Username Missing
    [InlineData("Jp")]  // Username < 3 
    [InlineData("😀aaA")] // Username has invalid characters
    [InlineData("adam\n")] // Special case: proves \z anchor works
    [InlineData("bad name")] // Generic bad names
    [InlineData("<script>")] 
    [InlineData("adam ")]
    [InlineData("josé")]
    [InlineData("     ")]
    public void Username_Invalid_HasError(string username)
    {
        var result = _validator.TestValidate(ValidRequest() with { Username = username });

        result.ShouldHaveValidationErrorFor(x => x.Username);
    }
    [Fact]
    public void Username_TooLong_HasError()
    {
        var tooLong = new string('x', UserConstraints.UsernameMaxLength + 1);

        var result = _validator.TestValidate(ValidRequest() with { Username = tooLong });

        result.ShouldHaveValidationErrorFor(x => x.Username);
    }
    // Valid Usernames:
    [Theory]
    [InlineData("Adam")]
    [InlineData("John_pork")]
    [InlineData("abc")]
    public void Username_Valid(string username)
    {
        var result = _validator.TestValidate(ValidRequest() with { Username = username });

        result.ShouldNotHaveValidationErrorFor(x => x.Username);
    }
    [Fact]
    public void Username_FitsBoundary()
    {
        var atLimit = new string('x', UserConstraints.UsernameMaxLength);

        var result = _validator.TestValidate(ValidRequest() with { Username = atLimit });

        result.ShouldNotHaveValidationErrorFor(x => x.Username);
    }
    // Invalid Email
    [Theory]
    [InlineData("")]    // Email Missing
    [InlineData("This-is-not-an-email")]    // Non Email
    [InlineData("     ")]
    [InlineData("@mail.com")]
    public void Email_Invalid_HasError(string email)
    {
        var result = _validator.TestValidate(ValidRequest() with { Email = email });

        result.ShouldHaveValidationErrorFor(x => x.Email);
    }
    [Fact]
    public void Email_TooLong_HasError()
    {
        var tooLong = new string('x', UserConstraints.EmailMaxLength - 9) + "@gmail.com";

        var result = _validator.TestValidate(ValidRequest() with { Email = tooLong });

        result.ShouldHaveValidationErrorFor(x => x.Email);
    }
    // Valid Email
    [Theory]
    [InlineData("example@gmail.com")]    
    [InlineData("I_am@gmail.com")]
    [InlineData("a@b")]
    [InlineData("John@Mail.COM")]
    public void Email_Valid(string email)
    {
        var result = _validator.TestValidate(ValidRequest() with { Email = email });

        result.ShouldNotHaveValidationErrorFor(x => x.Email);
    }
    [Fact]
    public void Email_FitsBoundary()
    {
        var atLimit = new string('x', UserConstraints.EmailMaxLength - 10) + "@gmail.com";

        var result = _validator.TestValidate(ValidRequest() with { Email = atLimit });

        result.ShouldNotHaveValidationErrorFor(x => x.Email);
    }
    // Invalid Password
    [Theory]
    [InlineData("")]    // Password Missing
    [InlineData("Passwo1")] // Password < 8
    [InlineData("password1")]   // Password has no uppercase
    [InlineData("PASSWORD1")]   // Password has no lowercase
    [InlineData("Password")]    // Password has no digits
    [InlineData("Passwörd1")]   // Password contains non Ascii-values
    [InlineData("Passw0rd\u007F")]  // Password contains DEL control character
    [InlineData("Passw0rd\t")]  // Password contains tab
    [InlineData("     ")]
    public void Password_Invalid_HasError(string password)
    {
        var result = _validator.TestValidate(ValidRequest() with { Password = password });

        result.ShouldHaveValidationErrorFor(x => x.Password);
    }
    [Fact]
    public void Password_TooLong_HasError()
    {
        var tooLong = "Aa1" + new string('x', UserConstraints.PasswordMaxLength - 2);
        var result = _validator.TestValidate(ValidRequest() with { Password = tooLong });

        result.ShouldHaveValidationErrorFor(x => x.Password);
    }
    // Valid Password
    [Theory]
    [InlineData("Pass word1")]
    [InlineData("Passw0rd")]

    public void Password_Valid(string password)
    {
        var result = _validator.TestValidate(ValidRequest() with { Password = password });

        result.ShouldNotHaveValidationErrorFor(x => x.Password);
    }
    [Fact]
    public void Password_FitsBoundary()
    {
        var atLimit = "Aa1" + new string('x', UserConstraints.PasswordMaxLength - 3);
        var result = _validator.TestValidate(ValidRequest() with { Password = atLimit });

        result.ShouldNotHaveValidationErrorFor(x => x.Password);
    }
    // Valid Request*
    [Fact]
    public void ValidRequest_PassesValidation()
    {
        var result = _validator.TestValidate(ValidRequest());

        Assert.True(result.IsValid);
    }
    // Null Request Fields
    [Fact]
    public void NullFields_HasErrors()
    {
        var result = _validator.TestValidate(new RegisterUserRequest(null!, null!, null!));

        result.ShouldHaveValidationErrorFor(x => x.Username);
        result.ShouldHaveValidationErrorFor(x => x.Email);
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }
}
