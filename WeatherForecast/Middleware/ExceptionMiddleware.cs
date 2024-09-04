using System.Net;
using WeatherForecast.App.Exceptions;
using WeatherForecast.Core.Boundaries.Responses;

namespace WeatherForecast.Api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate requestDelegate)
        {
            _next = requestDelegate;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                await SetResponse(context, exception);
            }
        }

        private static async Task SetResponse(HttpContext context, Exception exception)
        {
            var statusCode = HttpStatusCode.InternalServerError;
            var message = "An error occurred while processing your request. " + exception.Message;

            switch (exception)
            {
                case ArgumentException:
                case FluentValidation.ValidationException:
                case FormatException:
                    statusCode = HttpStatusCode.BadRequest;
                    message = exception.Message;
                    break;

                case WeatherNotFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    message = exception.Message;
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var errorResponse = new ErrorResponse { Message = message };
            await context.Response.WriteAsync(errorResponse.ToString());
        }
    }
}