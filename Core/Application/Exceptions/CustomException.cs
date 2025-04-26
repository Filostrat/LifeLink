namespace Application.Exceptions;

public class IdentityException : BaseException
{
	public IdentityException()
		: base("Identity", "Не правильний логін або пароль.") { }
}

public class UserNotFoundException : BaseException
{
	public UserNotFoundException()
		: base("UserNotFound", "Користувача не знайдено.") { }
}

public class EmailAlreadyConfirmedException : BaseException
{
	public EmailAlreadyConfirmedException()
		: base("EmailAlreadyConfirmed", "Почта вже підтвержена") { }
}

public class EmailConfirmationFailedException : BaseException
{
	public EmailConfirmationFailedException()
		: base("EmailConfirmationFailed", "Не вдалося підтвердити почту") { }
}

public class EmailNotSendException : BaseException
{
	public EmailNotSendException()
		: base("EmailNotSend", "Не вдалося відправити повідомлення на пошту") { }
}

public class InvalidCredentialsException : BaseException
{
	public InvalidCredentialsException(string email)
		: base("InvalidCredentials", $"Невірний логін або пароль для '{email}'.") { }
}

public class UsernameAlreadyExistsException : BaseException
{
	public UsernameAlreadyExistsException(string username)
		: base("UsernameAlreadyExists", $"Ім’я користувача '{username}' вже існує.") { }
}

public class EmailAlreadyExistsException : BaseException
{
	public EmailAlreadyExistsException(string email)
		: base("EmailAlreadyExists", $"Email '{email}' вже зареєстрований.") { }
}

public class UserCreationFailedException : BaseException
{
	public UserCreationFailedException()
		: base("UserCreationFailed", "Не вдалося зареєструвати користувача") { }
}

public class DonorNotFoundException : BaseException
{
	public DonorNotFoundException(string email)
		: base("DonorNotFound", $"Донор з email '{email}' не знайдений.") { }
}