using prueba_cajero_api_rest.Domain.Entities;
using prueba_cajero_api_rest.Domain.Exceptions;
using System.Net;

namespace prueba_cajero_api_rest.Middleware
{
    public class ErrorMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                context.Response.ContentType = "application/json";

                HttpStatusCode statusCode;

                switch (error)
                {
                    case NotFoundException:
                        statusCode = HttpStatusCode.NotFound;
                        break;

                    case UnauthorizedException:
                        statusCode = HttpStatusCode.Unauthorized;
                        break;

                    case KeyNotFoundException:
                        statusCode = HttpStatusCode.NotFound;
                        break;

                    case ArgumentException:
                        statusCode = HttpStatusCode.BadRequest;
                        break;

                    case InvalidOperationException:
                        statusCode = HttpStatusCode.BadRequest;
                        break;

                    default:
                        statusCode = HttpStatusCode.InternalServerError;
                        break;
                }

                context.Response.StatusCode = (int)statusCode;

                var result = ApiResponse<string>.Fail(error.Message);
                await context.Response.WriteAsJsonAsync(result);
            }
        }
    }
}
