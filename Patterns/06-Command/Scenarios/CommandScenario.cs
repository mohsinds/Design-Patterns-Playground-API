using DesignPatterns.Playground.Api.Domain;
using DesignPatterns.Playground.Api.Patterns._06_Command.Contracts;
using DesignPatterns.Playground.Api.Patterns._06_Command.Implementations;

namespace DesignPatterns.Playground.Api.Patterns._06_Command.Scenarios;

/// <summary>
/// Scenario class for demonstrating and testing Command pattern.
/// </summary>
public class CommandScenario
{
    private readonly ICommandHandler _handler;
    private readonly IOrderRepository _repository;
    private readonly ILoggerFactory _loggerFactory;
    private readonly ILogger<CommandScenario> _logger;
    
    public CommandScenario(
        ICommandHandler handler,
        IOrderRepository repository,
        ILoggerFactory loggerFactory,
        ILogger<CommandScenario> logger)
    {
        _handler = handler;
        _repository = repository;
        _loggerFactory = loggerFactory;
        _logger = logger;
    }
    
    public PatternDemoResponse RunDemo()
    {
        var results = new List<object>();
        
        // Create and execute a command
        var order = new Order(
            OrderId: "ORD-CMD-001",
            AccountId: "ACC-001",
            Symbol: "AAPL",
            Side: OrderSide.Buy,
            Quantity: 100,
            Price: 150m,
            Status: OrderStatus.Pending,
            CreatedAt: DateTime.UtcNow
        );
        
        var command = new PlaceOrderCommand(order, _repository, _loggerFactory.CreateLogger<PlaceOrderCommand>());
        var executeResult = _handler.ExecuteAsync(command).Result;
        
        results.Add(new
        {
            Action = "Execute Command",
            CommandId = command.CommandId,
            Result = executeResult
        });
        
        // Queue a command
        var order2 = new Order(
            OrderId: "ORD-CMD-002",
            AccountId: "ACC-001",
            Symbol: "MSFT",
            Side: OrderSide.Sell,
            Quantity: 50,
            Price: 300m,
            Status: OrderStatus.Pending,
            CreatedAt: DateTime.UtcNow
        );
        
        var command2 = new PlaceOrderCommand(order2, _repository, _loggerFactory.CreateLogger<PlaceOrderCommand>());
        _handler.QueueAsync(command2).Wait();
        
        results.Add(new
        {
            Action = "Queue Command",
            CommandId = command2.CommandId,
            QueueCount = ((CommandHandler)_handler).GetQueueCount()
        });
        
        // Get audit log
        var auditLog = ((CommandHandler)_handler).GetAuditLog();
        results.Add(new
        {
            Action = "Audit Log",
            Entries = auditLog.Take(5)
        });
        
        return new PatternDemoResponse(
            Pattern: "Command",
            Description: "Demonstrates command pattern: encapsulates requests as objects with retry, queue, and audit support.",
            Result: results,
            Metadata: new Dictionary<string, object>
            {
                ["RetrySupport"] = true,
                ["QueueSupport"] = true,
                ["AuditSupport"] = true,
                ["UndoSupport"] = true
            }
        );
    }
    
    public PatternTestResponse RunTest()
    {
        var checks = new List<TestCheck>();
        
        // Test 1: Command executes successfully
        var order = new Order(
            OrderId: "ORD-TEST-001",
            AccountId: "ACC-TEST",
            Symbol: "TEST",
            Side: OrderSide.Buy,
            Quantity: 10,
            Price: 100m,
            Status: OrderStatus.Pending,
            CreatedAt: DateTime.UtcNow
        );
        
        var command = new PlaceOrderCommand(order, _repository, _loggerFactory.CreateLogger<PlaceOrderCommand>());
        var result = _handler.ExecuteAsync(command).Result;
        
        checks.Add(new TestCheck(
            "Command Execution",
            result.Success,
            $"Command {command.CommandId} executed: {result.Success}"
        ));
        
        // Test 2: Order was persisted
        var retrievedOrder = _repository.GetByIdAsync(order.OrderId).Result;
        checks.Add(new TestCheck(
            "Order Persisted",
            retrievedOrder != null && retrievedOrder.OrderId == order.OrderId,
            $"Order {order.OrderId} was persisted"
        ));
        
        // Test 3: Undo works
        if (command.SupportsUndo)
        {
            var undoResult = command.UndoAsync().Result;
            checks.Add(new TestCheck(
                "Command Undo",
                undoResult.Success,
                $"Command undo: {undoResult.Success}"
            ));
        }
        
        // Test 4: Queue works
        var command2 = new PlaceOrderCommand(order, _repository, _loggerFactory.CreateLogger<PlaceOrderCommand>());
        _handler.QueueAsync(command2).Wait();
        var queueCount = ((CommandHandler)_handler).GetQueueCount();
        checks.Add(new TestCheck(
            "Command Queue",
            queueCount > 0,
            $"Commands in queue: {queueCount}"
        ));
        
        var allPassed = checks.All(c => c.Pass);
        
        return new PatternTestResponse(
            Pattern: "Command",
            Status: allPassed ? "PASS" : "FAIL",
            Checks: checks
        );
    }
}
