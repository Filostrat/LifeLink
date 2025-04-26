namespace Application.Exceptions;


public abstract class BaseException : Exception
{
	public Dictionary<string, string[]> Errors { get; }

	protected BaseException(string key, string message)
		: base(message)
	{
		Errors = new Dictionary<string, string[]>
			{
				{ key, new[] { message } }
			};
	}

	protected BaseException(Dictionary<string, string[]> errors)
	{
		Errors = errors;
	}
}
