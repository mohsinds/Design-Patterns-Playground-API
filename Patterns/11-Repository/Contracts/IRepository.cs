namespace DesignPatterns.Playground.Api.Patterns._11_Repository.Contracts;

/// <summary>
/// Generic repository interface.
/// Repository pattern: abstracts data access, enables testing, supports Unit of Work.
/// </summary>
public interface IRepository<TEntity, TKey> where TEntity : class
{
    /// <summary>
    /// Get entity by ID.
    /// </summary>
    Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Get all entities.
    /// </summary>
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Add entity.
    /// </summary>
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Update entity.
    /// </summary>
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Delete entity.
    /// </summary>
    Task DeleteAsync(TKey id, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Check if entity exists.
    /// </summary>
    Task<bool> ExistsAsync(TKey id, CancellationToken cancellationToken = default);
}
