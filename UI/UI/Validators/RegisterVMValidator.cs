using FluentValidation;
using UI.Models.Authentication;

namespace UI.Validators;

public class RegisterVMValidator : AbstractValidator<RegisterVM>
{
	public RegisterVMValidator()
	{
		RuleFor(x => x.FirstName)
			.NotEmpty().WithMessage("Ім'я обов'язкове для заповнення.");

		RuleFor(x => x.LastName)
			.NotEmpty().WithMessage("Прізвище обов'язкове для заповнення.");

		RuleFor(x => x.Email)
			.NotEmpty().WithMessage("Email обов'язковий.")
			.EmailAddress().WithMessage("Некоректний формат email.");

		RuleFor(x => x.BloodTypeId)
			.NotEmpty().WithMessage("Будь ласка, оберіть тип крові.")
			.Must(id => int.TryParse(id, out var parsed) && parsed > 0)
				.WithMessage("Некоректний тип крові.");

		RuleFor(x => x.UserName)
			.NotEmpty().WithMessage("Логін обов'язковий.")
			.MinimumLength(6).WithMessage("Логін повиннен мати довжину не менше 6 символів.");

		RuleFor(x => x.Password)
			.NotEmpty().WithMessage("Пароль обов'язковий.")
			.MinimumLength(8).WithMessage("Пароль повинен містити щонайменше 8 символів.")
			.Matches(@"[A-Z]").WithMessage("Пароль повинен містити хоча б одну велику літеру.")
			.Matches(@"\d").WithMessage("Пароль повинен містити хоча б одну цифру.")
			.Matches(@"[!@#$%^&*()_+={}\[\]|;:'"",.<>?/~`]").WithMessage("Пароль повинен містити хоча б один спеціальний символ.");

		RuleFor(x => x.ConfirmPassword)
			.NotEmpty().WithMessage("Підтвердження пароля обов'язкове.")
			.Equal(x => x.Password).WithMessage("Паролі не співпадають.");
	}
}
