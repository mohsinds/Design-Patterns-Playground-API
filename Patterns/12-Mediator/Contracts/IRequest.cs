namespace DesignPatterns.Playground.Api.Patterns._12_Mediator.Contracts;

/// <summary>
/// Request interface for mediator pattern.
/// Mediator pattern: reduces many-to-many dependencies by routing requests through a mediator.
/// </summary>
public interface IRequest<out TResponse>
{
}

/// <summary>
/// Request handler interface.
/// </summary>
public interface IRequestHandler<in TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken = default);
}
