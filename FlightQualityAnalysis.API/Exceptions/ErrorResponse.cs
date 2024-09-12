namespace FlightQualityAnalysis.API.Exceptions
{
    public class ErrorResponse
    {
        public int StatusCode { get; }
        public string Message { get; }
        public string? Details { get; }
        public string? StackTrace { get; }

        public ErrorResponse(int statusCode, string message, string? details = null, string? stackTrace = null)
        {
            StatusCode = statusCode;
            Message = message;
            Details = details;
            StackTrace = stackTrace;
        }
    }
}
