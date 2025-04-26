using Application.DTOs.Account.Requests;
using Application.Extensions.ValidationExtensions;
using FluentValidation;

namespace Application.DTOs.Account.Validators;

public class RegisterAccountRequestValidator : AbstractValidator<RegisterAccountRequestDTO>
{
	public RegisterAccountRequestValidator()
	{
		RuleFor(x => x.FirstName)
			.NotEmpty().WithMessage("First name is required.");

		RuleFor(x => x.LastName)
			.NotEmpty().WithMessage("Last name is required.");

		RuleFor(x => x.BloodTypeId)
			.NotEmpty().WithMessage("BloodTypeId is required.");

		RuleFor(x => x.Email)
			.NotEmpty().WithMessage("Email is required.")
			.EmailAddress().WithMessage("Email must be a valid email address.");

		RuleFor(x => x.UserName)
			.NotEmpty().WithMessage("Username is required.")
			.MinimumLength(6).WithMessage("Username must be at least 6 characters long.");

		RuleFor(x => x.Password)
			.ApplyPasswordRules();
	}
}