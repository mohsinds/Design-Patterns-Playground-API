namespace DesignPatterns.Playground.Api.Patterns._12_Mediator.Contracts;

/// <summary>
/// Mediator interface.
/// Mediator pattern: routes requests to appropriate handlers, reducing coupling.
/// </summary>
public interface IMediator
{
    /// <summary>
    /// Send a request and get response.
    /// </summary>
    Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
}
