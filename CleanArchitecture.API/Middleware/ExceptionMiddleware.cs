using CleanArchitecture.API.Errors;
using CleanArchitecture.Application.Exceptions;
using Newtonsoft.Json;
using System.Net;

namespace CleanArchitecture.API.Middleware
{
    public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
    {
        // this represents the completion of the request processing if there is no exception then will move
        // the the next process using _next
        private readonly RequestDelegate _next = next;
        // if there is en exception I want to logg a message
        private readonly ILogger<ExceptionMiddleware> _logger = logger;
        // useful when you want to know wether you are in a development or production environment
        private readonly IHostEnvironment _env = env;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(Exception e)
            {
                _logger.LogError(e, e.Message);
                context.Response.ContentType = "application/json";
                var statusCode = (int)HttpStatusCode.InternalServerError;
                var result = string.Empty;

                switch (e)
                {
                    case NotFoundException notFoundException:
                        statusCode = (int)HttpStatusCode.NotFound;
                        break;
                    case ValidationException validationException:
                        statusCode = (int)HttpStatusCode.BadRequest;
                        // serializing the validation errors collection to a JSON
                        var validationJson = JsonConvert.SerializeObject(validationException.Errors);
                        result = JsonConvert.SerializeObject(new CodeErrorException(statusCode, e.Message, validationJson));
                        break;
                    case BadRequestException badRequestException:
                        statusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    default:
                        break;
                }

                if(string.IsNullOrEmpty(result))
                    result = JsonConvert.SerializeObject(new CodeErrorException(statusCode, e.Message, e.StackTrace));

                // this was the first approach, replaced with the switch above
                // return different error info based in the environment (dev, prod)
                /*var response = _env.IsDevelopment()
                    ? new CodeErrorException((int)HttpStatusCode.InternalServerError, e.Message, e.StackTrace)
                    : new CodeErrorException((int)HttpStatusCode.InternalServerError);

                // defining options for the json will be sent to the client
                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                // creatin the json using the response info and the options
                var json = JsonSerializer.Serialize(response, options);*/

                context.Response.StatusCode = statusCode;

                // now sending the json to the client
                await context.Response.WriteAsync(result);
            }
        }

    }
}
