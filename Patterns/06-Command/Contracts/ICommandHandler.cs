namespace DesignPatterns.Playground.Api.Patterns._06_Command.Contracts;

/// <summary>
/// Command handler interface for executing commands with retry, audit, queue support.
/// </summary>
public interface ICommandHandler
{
    /// <summary>
    /// Execute a command with retry and audit.
    /// </summary>
    Task<CommandResult> ExecuteAsync(ICommand command, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Queue a command for later execution.
    /// </summary>
    Task QueueAsync(ICommand command, CancellationToken cancellationToken = default);
}
