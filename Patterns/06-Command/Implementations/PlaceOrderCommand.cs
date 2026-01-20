using System.Collections.Concurrent;
using DesignPatterns.Playground.Api.Domain;
using DesignPatterns.Playground.Api.Patterns._06_Command.Contracts;

namespace DesignPatterns.Playground.Api.Patterns._06_Command.Implementations;

/// <summary>
/// Place order command.
/// Command pattern: encapsulates order placement as a command object.
/// </summary>
public class PlaceOrderCommand : ICommand
{
    private readonly Order _order;
    private readonly IOrderRepository _repository;
    private readonly ILogger<PlaceOrderCommand> _logger;
    private Order? _originalOrder; // For undo
    
    public PlaceOrderCommand(
        Order order,
        IOrderRepository repository,
        ILogger<PlaceOrderCommand> logger)
    {
        _order = order;
        _repository = repository;
        _logger = logger;
        CommandId = $"CMD-{Guid.NewGuid():N}";
    }
    
    public string CommandId { get; }
    public bool SupportsUndo => true;
    
    public async Task<CommandResult> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Executing PlaceOrderCommand {CommandId} for order {OrderId}", 
                CommandId, _order.OrderId);
            
            // Store original state for undo
            _originalOrder = await _repository.GetByIdAsync(_order.OrderId, cancellationToken);
            
            // Place order
            await _repository.AddAsync(_order, cancellationToken);
            
            return new CommandResult(true, null, new { OrderId = _order.OrderId });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to execute PlaceOrderCommand {CommandId}", CommandId);
            return new CommandResult(false, ex.Message);
        }
    }
    
    public async Task<CommandResult> UndoAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            if (_originalOrder == null)
            {
                // Order didn't exist before, remove it
                await _repository.DeleteAsync(_order.OrderId, cancellationToken);
            }
            else
            {
                // Restore original state
                await _repository.UpdateAsync(_originalOrder, cancellationToken);
            }
            
            _logger.LogInformation("Undone PlaceOrderCommand {CommandId}", CommandId);
            return new CommandResult(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to undo PlaceOrderCommand {CommandId}", CommandId);
            return new CommandResult(false, ex.Message);
        }
    }
}

/// <summary>
/// Simple in-memory order repository for demo.
/// </summary>
public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(string orderId, CancellationToken cancellationToken = default);
    Task AddAsync(Order order, CancellationToken cancellationToken = default);
    Task UpdateAsync(Order order, CancellationToken cancellationToken = default);
    Task DeleteAsync(string orderId, CancellationToken cancellationToken = default);
}

/// <summary>
/// In-memory order repository implementation.
/// </summary>
public class InMemoryOrderRepository : IOrderRepository
{
    private readonly ConcurrentDictionary<string, Order> _orders = new();
    
    public Task<Order?> GetByIdAsync(string orderId, CancellationToken cancellationToken = default)
    {
        _orders.TryGetValue(orderId, out var order);
        return Task.FromResult(order);
    }
    
    public Task AddAsync(Order order, CancellationToken cancellationToken = default)
    {
        _orders[order.OrderId] = order;
        return Task.CompletedTask;
    }
    
    public Task UpdateAsync(Order order, CancellationToken cancellationToken = default)
    {
        _orders[order.OrderId] = order;
        return Task.CompletedTask;
    }
    
    public Task DeleteAsync(string orderId, CancellationToken cancellationToken = default)
    {
        _orders.TryRemove(orderId, out _);
        return Task.CompletedTask;
    }
}
