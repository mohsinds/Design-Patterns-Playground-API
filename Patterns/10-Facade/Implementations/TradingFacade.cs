using DesignPatterns.Playground.Api.Domain;
using DesignPatterns.Playground.Api.Patterns._02_FactoryMethod.Contracts;
using DesignPatterns.Playground.Api.Patterns._06_Command.Contracts;
using DesignPatterns.Playground.Api.Patterns._06_Command.Implementations;
using DesignPatterns.Playground.Api.Patterns._09_Observer.Contracts;
using DesignPatterns.Playground.Api.Patterns._09_Observer.Implementations;
using DesignPatterns.Playground.Api.Patterns._10_Facade.Contracts;

namespace DesignPatterns.Playground.Api.Patterns._10_Facade.Implementations;

/// <summary>
/// Trading facade implementation.
/// Facade pattern: simplifies complex subsystem (validation, risk, repository, events) behind simple interface.
/// Thread-safety: Uses thread-safe components internally.
/// </summary>
public class TradingFacade : ITradingFacade
{
    private readonly IOrderValidatorFactory _validatorFactory;
    private readonly IOrderRepository _repository;
    private readonly ICommandHandler _commandHandler;
    private readonly IEventBus _eventBus;
    private readonly ILoggerFactory _loggerFactory;
    private readonly ILogger<TradingFacade> _logger;
    
    public TradingFacade(
        IOrderValidatorFactory validatorFactory,
        IOrderRepository repository,
        ICommandHandler commandHandler,
        IEventBus eventBus,
        ILoggerFactory loggerFactory,
        ILogger<TradingFacade> logger)
    {
        _validatorFactory = validatorFactory;
        _repository = repository;
        _commandHandler = commandHandler;
        _eventBus = eventBus;
        _loggerFactory = loggerFactory;
        _logger = logger;
    }
    
    public async Task<PlaceOrderResult> PlaceOrderAsync(PlaceOrderRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            // 1. Create order
            var order = new Order(
                OrderId: $"ORD-{Guid.NewGuid():N}",
                AccountId: request.AccountId,
                Symbol: request.Symbol,
                Side: request.Side,
                Quantity: request.Quantity,
                Price: request.LimitPrice ?? 0m,
                Status: OrderStatus.Pending,
                CreatedAt: DateTime.UtcNow
            );
            
            // 2. Validate order
            var validator = _validatorFactory.CreateValidator(order);
            var validationResult = validator.Validate(order);
            
            if (!validationResult.IsValid)
            {
                return new PlaceOrderResult(false, null, validationResult.Errors);
            }
            
            // 3. Persist order (via command pattern)
            var commandLogger = _loggerFactory.CreateLogger<PlaceOrderCommand>();
            var command = new PlaceOrderCommand(order, _repository, commandLogger);
            var commandResult = await _commandHandler.ExecuteAsync(command, cancellationToken);
            
            if (!commandResult.Success)
            {
                return new PlaceOrderResult(false, null, new List<string> { commandResult.ErrorMessage ?? "Command failed" });
            }
            
            // 4. Publish domain event
            var orderPlacedEvent = new OrderPlacedEvent(
                order.OrderId,
                order.AccountId,
                order.Symbol,
                order.Quantity,
                order.Price,
                DateTime.UtcNow
            );
            
            await _eventBus.PublishAsync(orderPlacedEvent, cancellationToken);
            
            _logger.LogInformation("Order placed via facade: {OrderId}", order.OrderId);
            
            return new PlaceOrderResult(true, order);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error placing order via facade");
            return new PlaceOrderResult(false, null, new List<string> { ex.Message });
        }
    }
    
    public async Task<CancelOrderResult> CancelOrderAsync(CancelOrderRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            // 1. Get order
            var order = await _repository.GetByIdAsync(request.OrderId, cancellationToken);
            
            if (order == null)
            {
                return new CancelOrderResult(false, "Order not found");
            }
            
            if (order.AccountId != request.AccountId)
            {
                return new CancelOrderResult(false, "Unauthorized");
            }
            
            // 2. Update order status
            var cancelledOrder = order with
            {
                Status = OrderStatus.Cancelled,
                UpdatedAt = DateTime.UtcNow
            };
            
            await _repository.UpdateAsync(cancelledOrder, cancellationToken);
            
            // 3. Publish domain event
            var orderCancelledEvent = new OrderCancelledEvent(
                order.OrderId,
                order.AccountId,
                "User request",
                DateTime.UtcNow
            );
            
            await _eventBus.PublishAsync(orderCancelledEvent, cancellationToken);
            
            _logger.LogInformation("Order cancelled via facade: {OrderId}", order.OrderId);
            
            return new CancelOrderResult(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cancelling order via facade");
            return new CancelOrderResult(false, ex.Message);
        }
    }
}
