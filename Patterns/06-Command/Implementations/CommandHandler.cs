using System.Collections.Concurrent;
using DesignPatterns.Playground.Api.Patterns._06_Command.Contracts;

namespace DesignPatterns.Playground.Api.Patterns._06_Command.Implementations;

/// <summary>
/// Command handler with retry, audit, and queue support.
/// Command pattern: centralizes command execution with cross-cutting concerns.
/// Thread-safety: Uses concurrent collections for queue, safe for concurrent use.
/// 
/// HOW IT WORKS:
/// =============
/// 1. ExecuteAsync() wraps command execution with retry logic:
///    - Attempts up to 3 times (maxRetries = 3)
///    - Exponential backoff: 100ms, 200ms, 300ms delays
///    - Logs each attempt for audit
/// 
/// 2. Audit logging:
///    - Logs every command execution attempt
///    - Records: CommandId, Action (EXECUTE/SUCCESS/FAILED), RetryCount, Duration
///    - Thread-safe using lock (_auditLock)
/// 
/// 3. Queue support:
///    - QueueAsync() adds command to ConcurrentQueue
///    - Commands can be processed asynchronously later
///    - Thread-safe queue operations
/// 
/// 4. Retry strategy:
///    - If command fails, increments retryCount
///    - Waits with exponential backoff before retry
///    - After max retries, returns failure result
///    - Logs warnings for each retry attempt
/// </summary>
public class CommandHandler : ICommandHandler
{
    private readonly ILogger<CommandHandler> _logger;
    private readonly ConcurrentQueue<ICommand> _commandQueue = new();
    private readonly List<CommandAuditEntry> _auditLog = new();
    private readonly object _auditLock = new();
    
    public CommandHandler(ILogger<CommandHandler> logger)
    {
        _logger = logger;
    }
    
    public async Task<CommandResult> ExecuteAsync(ICommand command, CancellationToken cancellationToken = default)
    {
        var startTime = DateTime.UtcNow;
        var maxRetries = 3;
        var retryCount = 0;
        
        while (retryCount < maxRetries)
        {
            try
            {
                // Audit: log command execution
                AuditCommand(command, "EXECUTE", retryCount);
                
                var result = await command.ExecuteAsync(cancellationToken);
                
                if (result.Success)
                {
                    var duration = DateTime.UtcNow - startTime;
                    AuditCommand(command, "SUCCESS", retryCount, duration);
                    return result;
                }
                
                // Retry on failure
                retryCount++;
                if (retryCount < maxRetries)
                {
                    _logger.LogWarning("Command {CommandId} failed, retrying ({RetryCount}/{MaxRetries})",
                        command.CommandId, retryCount, maxRetries);
                    await Task.Delay(100 * retryCount, cancellationToken); // Exponential backoff
                }
                else
                {
                    AuditCommand(command, "FAILED", retryCount);
                }
            }
            catch (Exception ex)
            {
                retryCount++;
                _logger.LogError(ex, "Command {CommandId} threw exception (retry {RetryCount}/{MaxRetries})",
                    command.CommandId, retryCount, maxRetries);
                
                if (retryCount >= maxRetries)
                {
                    AuditCommand(command, "EXCEPTION", retryCount);
                    return new CommandResult(false, ex.Message);
                }
                
                await Task.Delay(100 * retryCount, cancellationToken);
            }
        }
        
        return new CommandResult(false, "Max retries exceeded");
    }
    
    public Task QueueAsync(ICommand command, CancellationToken cancellationToken = default)
    {
        _commandQueue.Enqueue(command);
        AuditCommand(command, "QUEUED", 0);
        _logger.LogInformation("Command {CommandId} queued", command.CommandId);
        return Task.CompletedTask;
    }
    
    private void AuditCommand(ICommand command, string action, int retryCount, TimeSpan? duration = null)
    {
        lock (_auditLock)
        {
            _auditLog.Add(new CommandAuditEntry(
                CommandId: command.CommandId,
                Action: action,
                Timestamp: DateTime.UtcNow,
                RetryCount: retryCount,
                Duration: duration
            ));
        }
    }
    
    public List<CommandAuditEntry> GetAuditLog() => _auditLog.ToList();
    
    public int GetQueueCount() => _commandQueue.Count;
}

/// <summary>
/// Command audit entry.
/// </summary>
public record CommandAuditEntry(
    string CommandId,
    string Action,
    DateTime Timestamp,
    int RetryCount,
    TimeSpan? Duration = null
);
