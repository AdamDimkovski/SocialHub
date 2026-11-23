using FluentValidation;
using SocialHub.Api.DTOs.Auth;
using SocialHub.Core.Constants;
using System.Text;

namespace SocialHub.Api.Validators;

public class RegisterUserRequestValidator : AbstractValidator<RegisterUserRequest>
{

    public RegisterUserRequestValidator()
    {
        RuleFor(x => x.Username)
            .Cascade(CascadeMode.Stop) 
            .NotEmpty().WithMessage("Username is required.")
            .MinimumLength(UserConstraints.UsernameMinLength).WithMessage($"Username cannot be less than {UserConstraints.UsernameMinLength} characters.")
            .MaximumLength(UserConstraints.UsernameMaxLength).WithMessage($"Username cannot exceed {UserConstraints.UsernameMaxLength} characters.")
            .Matches(@"^[a-zA-Z0-9_]+\z").WithMessage("Username may only contain letters, digits and underscores.");

        RuleFor(x => x.Email)
            .Cascade(CascadeMode.Stop) 
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("A valid email address is required.")
            .MaximumLength(UserConstraints.EmailMaxLength).WithMessage($"Email cannot exceed {UserConstraints.EmailMaxLength} characters.");

        RuleFor(x => x.Password)
            .Cascade(CascadeMode.Stop) 
            .NotEmpty().WithMessage("Password is required.")
            .Must(p => Encoding.UTF8.GetByteCount(p) <= 72).WithMessage("Password must be at most 72 bytes.")
            .Matches(@"^[\x20-\x7E]+\z").WithMessage("Password must only contain regularly used English characters.")
            .MinimumLength(UserConstraints.PasswordMinLength).WithMessage($"Password cannot be less than {UserConstraints.PasswordMinLength} characters.")
            .MaximumLength(UserConstraints.PasswordMaxLength).WithMessage($"Password cannot be greater than {UserConstraints.PasswordMaxLength} characters.")
            .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches(@"[0-9]").WithMessage("Password must contain at least one digit.");        
    }
}