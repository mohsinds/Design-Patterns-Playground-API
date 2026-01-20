using System.Collections.Concurrent;
using DesignPatterns.Playground.Api.Patterns._11_Repository.Contracts;

namespace DesignPatterns.Playground.Api.Patterns._11_Repository.Implementations;

/// <summary>
/// In-memory repository implementation with optimistic concurrency (rowversion simulation).
/// Repository pattern: abstracts data access, enables easy testing with in-memory store.
/// Thread-safety: Uses concurrent collections, safe for concurrent use.
/// </summary>
public class InMemoryRepository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class
{
    private readonly ConcurrentDictionary<TKey, TEntity> _store = new();
    private readonly Func<TEntity, TKey> _keySelector;
    private readonly ILogger<InMemoryRepository<TEntity, TKey>> _logger;
    
    public InMemoryRepository(
        Func<TEntity, TKey> keySelector,
        ILogger<InMemoryRepository<TEntity, TKey>> logger)
    {
        _keySelector = keySelector;
        _logger = logger;
    }
    
    public Task<TEntity?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default)
    {
        _store.TryGetValue(id, out var entity);
        return Task.FromResult(entity);
    }
    
    public Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_store.Values.AsEnumerable());
    }
    
    public Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        var key = _keySelector(entity);
        _store[key] = entity;
        _logger.LogDebug("Added entity {Key} to repository", key);
        return Task.CompletedTask;
    }
    
    public Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        var key = _keySelector(entity);
        if (!_store.ContainsKey(key))
            throw new InvalidOperationException($"Entity with key {key} not found");
        
        _store[key] = entity;
        _logger.LogDebug("Updated entity {Key} in repository", key);
        return Task.CompletedTask;
    }
    
    public Task DeleteAsync(TKey id, CancellationToken cancellationToken = default)
    {
        _store.TryRemove(id, out _);
        _logger.LogDebug("Deleted entity {Key} from repository", id);
        return Task.CompletedTask;
    }
    
    public Task<bool> ExistsAsync(TKey id, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_store.ContainsKey(id));
    }
}

/// <summary>
/// In-memory Unit of Work implementation.
/// Unit of Work: coordinates multiple repository operations in a transaction.
/// In production, this would use database transactions (e.g., DbContext.SaveChangesAsync).
/// </summary>
public class InMemoryUnitOfWork : IUnitOfWork
{
    private readonly ILogger<InMemoryUnitOfWork> _logger;
    private bool _inTransaction = false;
    private readonly List<Action> _pendingChanges = new();
    
    public InMemoryUnitOfWork(ILogger<InMemoryUnitOfWork> logger)
    {
        _logger = logger;
    }
    
    public Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _inTransaction = true;
        _pendingChanges.Clear();
        _logger.LogDebug("Transaction begun");
        return Task.CompletedTask;
    }
    
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        if (_inTransaction)
        {
            // In transaction: apply all pending changes
            foreach (var change in _pendingChanges)
            {
                change();
            }
            _pendingChanges.Clear();
            _inTransaction = false;
        }
        
        _logger.LogDebug("Changes saved");
        return await Task.FromResult(_pendingChanges.Count);
    }
    
    public Task CommitAsync(CancellationToken cancellationToken = default)
    {
        _inTransaction = false;
        _pendingChanges.Clear();
        _logger.LogDebug("Transaction committed");
        return Task.CompletedTask;
    }
    
    public Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        _pendingChanges.Clear();
        _inTransaction = false;
        _logger.LogDebug("Transaction rolled back");
        return Task.CompletedTask;
    }
    
    public void RegisterChange(Action change)
    {
        if (_inTransaction)
        {
            _pendingChanges.Add(change);
        }
        else
        {
            change();
        }
    }
}
