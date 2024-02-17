using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Application.Behaviours
{
    // this will execute within the hanlde methods of commands or queries (CreateStreamerCommandHandler this time) so if there is a error it will
    // be handle by this
    public class UnhandledExceptionBehaviour<TRequest, TResponse>(ILogger<TRequest> logger) : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<TRequest> _logger = logger;
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            try
            {
                return await next();
            }
            catch (Exception ex){
                var requestName = typeof(TRequest).Name;

                _logger.LogError(ex, "Application request: An exception was triggered for the request {Name} {@Request}", requestName, request);

                throw;
            }
        }
    }
}
