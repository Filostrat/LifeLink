using Api.Middlewares;


namespace Api.Extensions;

public static class ExceptionMiddlewareExtensions
{
	public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder builder)
	{
		builder.UseMiddleware<GlobalExceptionHandlerMiddleware>();
		return builder;
	}
}