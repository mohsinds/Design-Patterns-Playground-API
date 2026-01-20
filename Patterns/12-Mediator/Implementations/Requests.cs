using DesignPatterns.Playground.Api.Domain;
using DesignPatterns.Playground.Api.Patterns._11_Repository.Contracts;
using DesignPatterns.Playground.Api.Patterns._12_Mediator.Contracts;

namespace DesignPatterns.Playground.Api.Patterns._12_Mediator.Implementations;

/// <summary>
/// Get order request.
/// </summary>
public record GetOrderRequest(string OrderId) : IRequest<Order?>;

/// <summary>
/// Get order handler.
/// </summary>
public class GetOrderHandler : IRequestHandler<GetOrderRequest, Order?>
{
    private readonly IRepository<Order, string> _repository;
    private readonly ILogger<GetOrderHandler> _logger;
    
    public GetOrderHandler(
        IRepository<Order, string> repository,
        ILogger<GetOrderHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }
    
    public async Task<Order?> HandleAsync(GetOrderRequest request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Handling GetOrderRequest for {OrderId}", request.OrderId);
        return await _repository.GetByIdAsync(request.OrderId, cancellationToken);
    }
}

/// <summary>
/// Create order request.
/// </summary>
public record CreateOrderRequest(
    string AccountId,
    string Symbol,
    OrderSide Side,
    decimal Quantity,
    decimal Price
) : IRequest<Order>;

/// <summary>
/// Create order handler.
/// </summary>
public class CreateOrderHandler : IRequestHandler<CreateOrderRequest, Order>
{
    private readonly IRepository<Order, string> _repository;
    private readonly ILogger<CreateOrderHandler> _logger;
    
    public CreateOrderHandler(
        IRepository<Order, string> repository,
        ILogger<CreateOrderHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }
    
    public async Task<Order> HandleAsync(CreateOrderRequest request, CancellationToken cancellationToken = default)
    {
        var order = new Order(
            OrderId: $"ORD-{Guid.NewGuid():N}",
            AccountId: request.AccountId,
            Symbol: request.Symbol,
            Side: request.Side,
            Quantity: request.Quantity,
            Price: request.Price,
            Status: OrderStatus.Pending,
            CreatedAt: DateTime.UtcNow
        );
        
        await _repository.AddAsync(order, cancellationToken);
        _logger.LogInformation("Created order {OrderId} via mediator", order.OrderId);
        
        return order;
    }
}
