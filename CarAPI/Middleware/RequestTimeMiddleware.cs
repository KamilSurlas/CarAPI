using System.Diagnostics;

namespace CarAPI.Middleware
{
    public class RequestTimeMiddleware : IMiddleware
    {
        private Stopwatch _stopwatch;
        private readonly ILogger _logger;
        public RequestTimeMiddleware(ILogger<RequestTimeMiddleware> logger)
        {
            _stopwatch = new Stopwatch();
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            _stopwatch.Start();  
            await next.Invoke(context);
            _stopwatch.Stop();

            var miliseconds = _stopwatch.ElapsedMilliseconds;
            if (miliseconds / 1000.0D > 5)
            {
                var info = $"Request [{context.Request.Method}] at {context.Request.Path} took longer than usually ({miliseconds} ms) ";
            
                _logger.LogInformation(info);
            }
        }
    }
}
