using Serilog;

namespace WEB_253504_Novikov.UI.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await _next(context);

            if (context.Response.StatusCode / 100 != 2)
            {
                var requestedUrl = context.Request.Path + context.Request.QueryString;
                var statusCode = context.Response.StatusCode;

                Log.Information("---> request {Url} returns {StatusCode}", requestedUrl, statusCode);
            }
        }
    }
}
