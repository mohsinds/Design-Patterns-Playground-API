namespace DesignPatterns.Playground.Api.Patterns._06_Command.Contracts;

/// <summary>
/// Command interface.
/// Command pattern: encapsulates request as an object, enabling queuing, logging, undo, etc.
/// </summary>
public interface ICommand
{
    /// <summary>
    /// Execute the command.
    /// </summary>
    Task<CommandResult> ExecuteAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Undo the command (if supported).
    /// </summary>
    Task<CommandResult> UndoAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Get command ID for tracking/auditing.
    /// </summary>
    string CommandId { get; }
    
    /// <summary>
    /// Check if command supports undo.
    /// </summary>
    bool SupportsUndo { get; }
}

/// <summary>
/// Command execution result.
/// </summary>
public record CommandResult(bool Success, string? ErrorMessage = null, object? Data = null);
