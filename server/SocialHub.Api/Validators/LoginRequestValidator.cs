using FluentValidation;
using SocialHub.Api.DTOs.Auth;
using SocialHub.Core.Constants;

namespace SocialHub.Api.Validators;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Identifier)
            .Cascade(CascadeMode.Stop) 
            .NotEmpty().WithMessage("Username or email is required.")
            .MaximumLength(UserConstraints.EmailMaxLength).WithMessage($"Username cannot exceed 100 characters.");

        RuleFor(x => x.Password)
            .Cascade(CascadeMode.Stop) 
            .NotEmpty().WithMessage("Password is required.")
            .MaximumLength(UserConstraints.PasswordMaxLength).WithMessage("Password cannot exceed 50 characters.");     
    }
}