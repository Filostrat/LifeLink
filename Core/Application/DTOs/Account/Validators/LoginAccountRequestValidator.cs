using Application.DTOs.Account.Requests;
using Application.Extensions.ValidationExtensions;
using FluentValidation;

namespace Application.DTOs.Account.Validators;

public class LoginAccountRequestValidator : AbstractValidator<LoginAccountRequestDTO>
{
	public LoginAccountRequestValidator()
	{
		RuleFor(x => x.Email)
			.NotEmpty().WithMessage("Email is required.")
			.EmailAddress().WithMessage("Email must be a valid email address.");

		RuleFor(x => x.Password)
			.ApplyPasswordRules();
	}
}