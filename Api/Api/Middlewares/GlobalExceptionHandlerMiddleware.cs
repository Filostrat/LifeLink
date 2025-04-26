using Application.Exceptions;


namespace Api.Middlewares;

internal class GlobalExceptionHandlerMiddleware
{
	private readonly RequestDelegate _next;
	private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

	public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
	{
		_logger = logger;
		_next = next;
	}

	public async Task InvokeAsync(HttpContext context)
	{
		try
		{
			await _next(context);
		}
		catch (BaseException ex)
		{
			await HandleExceptionAsync(context, GetStatusCode(ex), ex.Errors);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "An unhandled exception occurred while processing the request.");
			await HandleExceptionAsync(context, StatusCodes.Status500InternalServerError, new Dictionary<string, string[]>
			{
				{ "GeneralError", ["An unexpected error occurred."] }
			});
		}
	}

	private static Task HandleExceptionAsync(HttpContext context, int statusCode, Dictionary<string, string[]> errors)
	{
		context.Response.ContentType = "application/json";
		context.Response.StatusCode = statusCode;

		var response = new
		{
			errors,
			type = "https://tools.ietf.org/html/rfc9110",
			title = "One or more errors occurred.",
			status = statusCode,
			traceId = context.TraceIdentifier
		};

		return context.Response.WriteAsJsonAsync(response);
	}

	private static int GetStatusCode(BaseException exception)
	{
		return exception switch
		{
			UserNotFoundException => StatusCodes.Status404NotFound,
			DonorNotFoundException => StatusCodes.Status404NotFound,
			InvalidCredentialsException => StatusCodes.Status401Unauthorized,
			UsernameAlreadyExistsException => StatusCodes.Status409Conflict,
			EmailAlreadyExistsException => StatusCodes.Status409Conflict,
			UserCreationFailedException => StatusCodes.Status400BadRequest,
			EmailConfirmationFailedException => StatusCodes.Status400BadRequest,
			EmailAlreadyConfirmedException => StatusCodes.Status400BadRequest,
			_ => StatusCodes.Status500InternalServerError
		};
	}
}