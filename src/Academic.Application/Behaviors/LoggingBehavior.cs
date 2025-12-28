using Academic.Application.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Academic.Application.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IAppLogger<TRequest> _logger;

        public LoggingBehavior(IAppLogger<TRequest> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;

            // Log before the handler executes
            _logger.LogInformation("Handling {RequestName} {@Request}", requestName, request);

            try
            {
                // Execute the actual Command Handler logic
                var response = await next();

                // Log after successful execution
                _logger.LogInformation("Handled {RequestName} successfully.", requestName);

                return response;
            }
            catch (Exception ex)
            {
                // Log any exceptions that occur
                _logger.LogError(ex, "Request: Failure handling {RequestName}", requestName);
                throw; // Re-throw the exception to be caught by API middleware
            }
        }
    }
}