namespace CleanArchitecture.API.Errors
{
    public class CodeErrorException(int statusCode, string? message = null, string? details = null) : CodeErrorResponse(statusCode, message)
    {
        public string? Details { get; set; } = details;
    }
}
