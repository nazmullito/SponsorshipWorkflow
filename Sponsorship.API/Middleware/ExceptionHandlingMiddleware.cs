using Sponsorship.Application.Common;
using Sponsorship.Application.Common.Exceptions;
using System.Net;
using System.Text.Json;

namespace Sponsorship.API.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var response = new ApiResponse<object>();

            switch (exception)
            {
                case NotFoundException:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;

                    response = ApiResponse<object>.FailureResponse(exception.Message);

                    break;

                case BadRequestException:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                    response = ApiResponse<object>.FailureResponse(exception.Message);

                    break;

                case ForbiddenException:
                    context.Response.StatusCode = (int)HttpStatusCode.Forbidden;

                    response = ApiResponse<object>.FailureResponse(exception.Message);

                    break;

                default:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                    response = ApiResponse<object>.FailureResponse("An unexpected error occurred");

                    break;
            }

            var json = JsonSerializer.Serialize(response);

            await context.Response.WriteAsync(json);
        }
    }
}
