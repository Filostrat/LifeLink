using FluentValidation;

namespace Application.Extensions.ValidationExtensions;

public static class PasswordValidationExtensions
{
	public static IRuleBuilderOptions<T, string> ApplyPasswordRules<T>(this IRuleBuilder<T, string> ruleBuilder)
	{
		return ruleBuilder
			.NotEmpty().WithMessage("Password is required.")
			.MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
			.Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
			.Matches(@"\d").WithMessage("Password must contain at least one digit.")
			.Matches(@"[!@#$%^&*()_+={}\[\]|;:'"",.<>?/~`]").WithMessage("Password must contain at least one special character.");
	}
}