namespace DesignPatterns.Playground.Api.Patterns._09_Observer.Contracts;

/// <summary>
/// Event handler interface.
/// </summary>
public interface IEventHandler<in TEvent> where TEvent : IDomainEvent
{
    /// <summary>
    /// Handle the event.
    /// </summary>
    Task HandleAsync(TEvent @event, CancellationToken cancellationToken = default);
}
