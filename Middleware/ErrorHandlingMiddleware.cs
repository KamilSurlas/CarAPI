using CarAPI.Exceptions;

namespace CarAPI.Middleware
{
    public class ErrorHandlingMiddleware : IMiddleware
    {
        private readonly ILogger _logger;

        public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger)
        {
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
			try
			{
				await next.Invoke(context);
			}
            catch (ContentNotFoundException exc)
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync(exc.Message);
            }
            catch (InvalidInsuranceDate exc)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync(exc.Message);
            }
            catch (Exception exc)
			{
                _logger.LogError(exc, exc.Message);
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync("Ups, something went wrong");
			}
            
        }
    }
}
