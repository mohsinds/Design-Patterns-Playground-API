using DesignPatterns.Playground.Api.Patterns._12_Mediator.Contracts;

namespace DesignPatterns.Playground.Api.Patterns._12_Mediator.Implementations;

/// <summary>
/// Simple mediator implementation.
/// Mediator pattern: routes requests to handlers, reducing many-to-many dependencies.
/// Thread-safety: Uses service provider resolution, safe for concurrent use.
/// </summary>
public class Mediator : IMediator
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<Mediator> _logger;
    
    public Mediator(IServiceProvider serviceProvider, ILogger<Mediator> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }
    
    public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        var requestType = request.GetType();
        var responseType = typeof(TResponse);
        
        // Find handler type: IRequestHandler<TRequest, TResponse>
        var handlerType = typeof(IRequestHandler<,>).MakeGenericType(requestType, responseType);
        
        // Resolve handler from DI
        var handler = _serviceProvider.GetService(handlerType);
        
        if (handler == null)
        {
            throw new InvalidOperationException($"No handler found for request type {requestType.Name}");
        }
        
        _logger.LogDebug("Mediator routing request {RequestType} to handler {HandlerType}",
            requestType.Name, handler.GetType().Name);
        
        // Invoke handler
        var handleMethod = handlerType.GetMethod("HandleAsync");
        var task = (Task<TResponse>)handleMethod!.Invoke(handler, new object[] { request, cancellationToken })!;
        
        return await task;
    }
}
