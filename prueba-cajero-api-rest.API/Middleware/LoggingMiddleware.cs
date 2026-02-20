using System.Diagnostics;

namespace prueba_cajero_api_rest.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var requestId = Guid.NewGuid().ToString();
            context.TraceIdentifier = requestId;
            var user = context.User.Identity?.Name ?? "anonymous";

            _logger.LogInformation("Request {RequestId} started by {User} - {Method} {Path}",
                requestId, user, context.Request.Method, context.Request.Path);

            var originalBodyStream = context.Response.Body;
            await using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            var stopwatch = Stopwatch.StartNew();

            try
            {
                await _next(context);

                stopwatch.Stop();

                context.Response.Body.Seek(0, SeekOrigin.Begin);
                var responseText = await new StreamReader(context.Response.Body).ReadToEndAsync();
                context.Response.Body.Seek(0, SeekOrigin.Begin);

                _logger.LogInformation("Request {RequestId} finished with status {StatusCode} in {ElapsedMilliseconds}ms. Response Body: {ResponseBody}",
                    requestId, context.Response.StatusCode, stopwatch.ElapsedMilliseconds, responseText);

                await responseBody.CopyToAsync(originalBodyStream);
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.LogError(ex, "Request {RequestId} failed after {ElapsedMilliseconds}ms", requestId, stopwatch.ElapsedMilliseconds);
                throw;
            }
            finally
            {
                context.Response.Body = originalBodyStream;
            }
        }
    }
}
