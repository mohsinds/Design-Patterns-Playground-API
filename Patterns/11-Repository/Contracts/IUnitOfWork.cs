namespace DesignPatterns.Playground.Api.Patterns._11_Repository.Contracts;

/// <summary>
/// Unit of Work interface.
/// Unit of Work pattern: manages transactions across multiple repositories.
/// Useful when: multiple entities must be saved atomically, or when you need to coordinate
/// multiple repositories in a single transaction.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Save all changes (commit transaction).
    /// </summary>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Begin a transaction.
    /// </summary>
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Commit transaction.
    /// </summary>
    Task CommitAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Rollback transaction.
    /// </summary>
    Task RollbackAsync(CancellationToken cancellationToken = default);
}
