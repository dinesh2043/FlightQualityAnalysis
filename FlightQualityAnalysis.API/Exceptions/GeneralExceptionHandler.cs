using Microsoft.AspNetCore.Diagnostics;

namespace FlightQualityAnalysis.API.Exceptions
{
    public class GeneralExceptionHandler : IExceptionHandler
    {
        private readonly ILogger _logger;
        private readonly IHostEnvironment _env;

        public GeneralExceptionHandler(ILogger<GeneralExceptionHandler> logger, IHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var (statusCode, errorResponse) = CreateErrorResponse(exception);

            // Log the exception details
            _logger.LogError(exception, errorResponse.Message);

            httpContext.Response.StatusCode = statusCode;
            httpContext.Response.ContentType = "application/json";

            // Write a structured error response
            await httpContext.Response.WriteAsJsonAsync(errorResponse, cancellationToken);
            return true;
        }
        private (int, ErrorResponse) CreateErrorResponse(Exception exception)
        {
            // Determine the status code and message based on the exception type
            return exception switch
            {
                ArgumentNullException => (400, new ErrorResponse(400, "Required parameter is missing.")),
                UnauthorizedAccessException => (403, new ErrorResponse(403, "Access is denied.")),
                KeyNotFoundException => (404, new ErrorResponse(404, "Resource not found.")),
                // Add more exception types as needed
                _ => (500, CreateInternalServerErrorResponse(exception))
            };
        }

        private ErrorResponse CreateInternalServerErrorResponse(Exception exception)
        {
            // In development, show detailed error information
            if (_env.IsDevelopment())
            {
                return new ErrorResponse(500, "An internal server error occurred.", exception.Message, exception.StackTrace);
            }

            // In production, hide sensitive details
            return new ErrorResponse(500, "Something went wrong. Please try again later.");
        }
    }
}
