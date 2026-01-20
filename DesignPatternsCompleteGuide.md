# Design Patterns Complete Guide

*Comprehensive reference for all 16 design patterns with use-cases, implementation guidance, and best practices*

**By Mohsin Rasheed**

---

## Table of Contents

1. [Introduction](#introduction)
2. [Pattern Overview by Importance](#pattern-overview-by-importance)
3. [Complete Pattern Reference](#complete-pattern-reference)
4. [Quick Decision Matrix](#quick-decision-matrix)
5. [Common Pitfalls and Anti-Patterns](#common-pitfalls-and-anti-patterns)

---

## Introduction

This guide provides comprehensive information about all 16 design patterns implemented in this project. Each pattern includes:
- **When to Use**: Specific use-cases and scenarios
- **How to Use**: Implementation guidance and code structure
- **When NOT to Use**: Common mistakes and anti-patterns
- **Important Notes**: Enterprise considerations, testing, scalability
- **Importance Score**: Ranking from 1-10 based on enterprise usage and interview frequency

---

## Pattern Overview by Importance

| Rank | Pattern | Score | Category | Primary Use-Case |
|------|---------|-------|----------|------------------|
| 1 | Strategy | 10/10 | Behavioral | Runtime algorithm selection |
| 2 | Observer | 9/10 | Behavioral | Event-driven architecture |
| 3 | Command | 9/10 | Behavioral | Undo/redo, queuing, audit |
| 4 | Factory Method | 8/10 | Creational | Conditional object creation |
| 5 | Builder | 8/10 | Creational | Complex object construction |
| 6 | Decorator | 8/10 | Structural | Cross-cutting concerns |
| 7 | Repository | 8/10 | Architectural | Data access abstraction |
| 8 | State | 8/10 | Behavioral | State machine behavior |
| 9 | Singleton | 7/10 | Creational | One instance per app |
| 10 | Abstract Factory | 7/10 | Creational | Compatible object families |
| 11 | Adapter | 7/10 | Structural | Interface compatibility |
| 12 | Facade | 7/10 | Structural | Subsystem simplification |
| 13 | Mediator | 7/10 | Behavioral | Component decoupling |
| 14 | Chain of Responsibility | 7/10 | Behavioral | Processing pipelines |
| 15 | Strategy Advanced | 10/10 | Behavioral | Dynamic provider selection |
| 16 | Prototype | 5/10 | Creational | Object cloning |

---

## Complete Pattern Reference

### 1. Strategy Pattern (10/10) ⭐⭐⭐⭐⭐

#### When to Use

✅ **Perfect for:**
- Multiple algorithms for the same task
- Runtime algorithm selection based on data
- Avoiding giant if/switch statements
- Making algorithms interchangeable
- Business rules that change frequently

**Real-World Use-Cases:**
- **Payment Processing**: Credit card vs ACH vs Wire transfer selection
- **Pricing Algorithms**: Market price vs Limit price vs VWAP vs Risk-adjusted
- **Risk Calculations**: VaR vs CVaR vs Stress testing models
- **Routing Strategies**: Smart order routing vs TWAP vs VWAP
- **Tax Calculations**: Different rules by jurisdiction
- **Compression Algorithms**: ZIP vs GZIP vs BZIP2 based on file type

#### How to Use

**Structure:**
```csharp
// 1. Define strategy interface
public interface IPricingStrategy {
    decimal CalculatePrice(Order order, Quote marketQuote);
    string StrategyName { get; }
}

// 2. Implement concrete strategies
public class MarketPriceStrategy : IPricingStrategy { ... }
public class LimitPriceStrategy : IPricingStrategy { ... }
public class VwapPricingStrategy : IPricingStrategy { ... }

// 3. Create selector (optional)
public class PricingStrategySelector {
    public IPricingStrategy SelectStrategy(Order order) {
        // Selection logic based on order characteristics
        if (orderValue > 500000) return _riskAdjustedStrategy;
        if (hasLimitPrice) return _limitPriceStrategy;
        return _marketPriceStrategy;
    }
}

// 4. Use strategy
var strategy = _selector.SelectStrategy(order);
var price = strategy.CalculatePrice(order, marketQuote);
```

**Key Implementation Points:**
- All strategies implement the same interface
- Strategies are stateless (can be singleton)
- Selection logic can be in a separate selector class
- Strategies are interchangeable at runtime

#### When NOT to Use

❌ **Don't use when:**
- You only have one algorithm (over-engineering)
- Selection is compile-time only (use inheritance)
- Simple conditional logic (if/switch is fine)
- Algorithms aren't interchangeable
- Performance is critical and polymorphism overhead matters

**Anti-Patterns:**
- Strategy for every conditional branch
- Strategies with shared mutable state
- Strategies that know about each other

#### Important Notes & Tips

**Enterprise Considerations:**
- Strategies are stateless - safe for multi-instance deployment
- Each pod can use different strategies independently
- No shared state between strategy instances
- Selection based on request data, not configuration

**Testing:**
- Test each strategy independently
- Mock strategies for testing selector logic
- Test strategy interchangeability
- Verify selection logic with different inputs

**Performance:**
- Strategies are lightweight (no state)
- Polymorphism overhead is minimal
- Consider caching strategy selection if expensive

**Interview Tip:** This is the #1 most asked pattern. Be ready to explain how you'd switch between algorithms at runtime without modifying existing code.

---

### 2. Observer Pattern (9/10) ⭐⭐⭐⭐⭐

#### When to Use

✅ **Perfect for:**
- Notifying multiple components about state changes
- Event-driven architecture
- Loose coupling between components
- Decoupling event publishers from subscribers
- Microservices communication

**Real-World Use-Cases:**
- **Domain Events**: Order placed, Payment processed, Trade executed
- **Market Data Distribution**: Price updates to multiple subscribers
- **Audit Logging**: Business events trigger audit entries
- **Notification Systems**: User actions trigger email/SMS/push
- **UI Updates**: Model changes notify multiple views
- **Cache Invalidation**: Data changes notify cache layers

#### How to Use

**Structure:**
```csharp
// 1. Define event interface
public interface IDomainEvent {
    string EventId { get; }
    string EventType { get; }
    DateTime OccurredAt { get; }
}

// 2. Define event bus interface
public interface IEventBus {
    Task PublishAsync<TEvent>(TEvent @event) where TEvent : IDomainEvent;
    void Subscribe<TEvent>(IEventHandler<TEvent> handler);
}

// 3. Define event handler interface
public interface IEventHandler<TEvent> where TEvent : IDomainEvent {
    Task HandleAsync(TEvent @event);
}

// 4. Implement events
public record OrderPlacedEvent(Order Order) : IDomainEvent { ... }

// 5. Implement handlers
public class OrderPlacedEventHandler : IEventHandler<OrderPlacedEvent> {
    public async Task HandleAsync(OrderPlacedEvent @event) {
        // Handle event
    }
}

// 6. Publish and subscribe
_eventBus.Subscribe<OrderPlacedEvent>(new OrderPlacedEventHandler());
await _eventBus.PublishAsync(new OrderPlacedEvent(order));
```

**Key Implementation Points:**
- Event bus maintains dictionary of event types to handlers
- Handlers subscribe to specific event types
- Publishing notifies all subscribed handlers
- Handlers should be idempotent (handle duplicate events)

#### When NOT to Use

❌ **Don't use when:**
- One-to-one notifications (use callbacks)
- Order of notifications is critical (use workflow engine)
- You need guaranteed delivery (use message queues with persistence)
- Observers need to know about each other (defeats the purpose)
- Synchronous execution is required

**Anti-Patterns:**
- Forgetting to unsubscribe (memory leaks)
- Handlers that throw exceptions (affect other handlers)
- Circular dependencies between observers

#### Important Notes & Tips

**Enterprise Considerations:**
- Use distributed message brokers (Kafka, RabbitMQ) in production
- Each pod publishes/subscribes to events
- Event handlers must be **idempotent** (handle duplicate events)
- Use **outbox pattern** for reliable event publishing
- Consider event ordering and partitioning

**Testing:**
- Subscribe test handlers
- Publish events, verify handlers called
- Test event ordering
- Test handler failures don't affect others

**Performance:**
- Async event handling prevents blocking
- Consider batching events
- Monitor handler execution time

**Interview Tip:** This pattern is critical for microservices communication. Be ready to explain how events flow through a distributed system.

---

### 3. Command Pattern (9/10) ⭐⭐⭐⭐⭐

#### When to Use

✅ **Perfect for:**
- Undo/redo functionality
- Queuing operations
- Audit trails and compliance
- Transaction support
- Macro operations (command sequences)
- Retry logic

**Real-World Use-Cases:**
- **Trading Platforms**: Place, cancel, replace orders with undo
- **Text Editors**: Undo/redo typing
- **Financial Transactions**: Rollback on failure
- **Workflow Engines**: Business process steps
- **API Request Queuing**: Process later if system busy
- **Batch Processing**: Queue commands for batch execution

#### How to Use

**Structure:**
```csharp
// 1. Define command interface
public interface ICommand {
    string CommandId { get; }
    bool SupportsUndo { get; }
    Task<CommandResult> ExecuteAsync();
    Task<CommandResult> UndoAsync();
}

// 2. Implement concrete command
public class PlaceOrderCommand : ICommand {
    private readonly Order _order;
    private readonly IOrderRepository _repository;
    private Order? _originalOrder; // For undo
    
    public async Task<CommandResult> ExecuteAsync() {
        _originalOrder = await _repository.GetByIdAsync(_order.OrderId);
        await _repository.AddAsync(_order);
        return new CommandResult(success: true);
    }
    
    public async Task<CommandResult> UndoAsync() {
        if (_originalOrder == null) {
            await _repository.DeleteAsync(_order.OrderId);
        } else {
            await _repository.UpdateAsync(_originalOrder);
        }
        return new CommandResult(success: true);
    }
}

// 3. Command handler with cross-cutting concerns
public class CommandHandler {
    public async Task<CommandResult> ExecuteAsync(ICommand command) {
        // Retry logic, audit logging, queue support
        var result = await command.ExecuteAsync();
        AuditCommand(command, "EXECUTE");
        return result;
    }
}
```

**Key Implementation Points:**
- Commands encapsulate operations and data
- Commands store original state for undo
- Command handler adds retry, audit, queue support
- Commands can be serialized for queuing

#### When NOT to Use

❌ **Don't use when:**
- Simple method calls (over-engineering)
- No undo/queue/audit needed
- Operations are stateless and don't need logging
- Only synchronous execution required
- Performance is critical (adds overhead)

**Anti-Patterns:**
- Creating commands for every method call
- Commands with side effects outside undo scope
- Commands that depend on each other

#### Important Notes & Tips

**Enterprise Considerations:**
- Commands queued in Kafka/RabbitMQ for distributed systems
- Command IDs must be globally unique (UUID)
- Handlers must be **idempotent** (handle duplicate commands)
- Use **outbox pattern** for reliable delivery
- Commands can span multiple services (saga pattern)

**Testing:**
- Test command execution
- Test undo functionality
- Test command queuing
- Mock command handlers
- Test retry logic

**Performance:**
- Commands add overhead (object creation)
- Consider command pooling for high-throughput
- Batch commands when possible

**Interview Tip:** This pattern is essential for FinTech. Be ready to explain how you'd implement undo/redo and audit trails.

---

### 4. Factory Method Pattern (8/10) ⭐⭐⭐⭐

#### When to Use

✅ **Perfect for:**
- Object creation depends on input/conditions
- Want to decouple creation from usage
- Multiple product types with same interface
- Creation logic may change frequently

**Real-World Use-Cases:**
- **Order Validators**: Standard vs Large vs High-frequency based on order
- **Report Generators**: PDF vs Excel vs CSV based on request
- **Payment Processors**: Credit card vs ACH based on payment method
- **Risk Calculators**: Different models based on asset type
- **Notification Channels**: Email vs SMS vs Push based on user preference
- **Database Connections**: Different connection types based on environment

#### How to Use

**Structure:**
```csharp
// 1. Define product interface
public interface IOrderValidator {
    bool Validate(Order order);
}

// 2. Define factory interface
public interface IOrderValidatorFactory {
    IOrderValidator CreateValidator(Order order);
}

// 3. Implement products
public class StandardOrderValidator : IOrderValidator { ... }
public class LargeOrderValidator : IOrderValidator { ... }

// 4. Implement factory
public class OrderValidatorFactory : IOrderValidatorFactory {
    public IOrderValidator CreateValidator(Order order) {
        var orderValue = order.Quantity * order.Price;
        if (orderValue >= 100000) {
            return new LargeOrderValidator();
        }
        return new StandardOrderValidator();
    }
}
```

**Key Implementation Points:**
- Factory encapsulates creation logic
- Factory receives input to decide which product to create
- Products implement common interface
- Factory returns interface, not concrete type

#### When NOT to Use

❌ **Don't use when:**
- Simple object creation (use constructor)
- Creation logic is trivial (over-engineering)
- Only one product type
- You need runtime type selection (use Abstract Factory)
- Creation doesn't depend on input

**Anti-Patterns:**
- Factory that just wraps `new` keyword
- Factory with complex conditional logic (consider Strategy)
- Factory creating incompatible products

#### Important Notes & Tips

**Enterprise Considerations:**
- Factories are stateless - safe for multi-instance
- Each pod creates objects independently
- Factory selection should be deterministic (same input = same output)
- Consider caching factory decisions if expensive

**Testing:**
- Mock factory for testing consumers
- Test factory selection logic
- Test created products independently
- Test factory with edge cases

**Performance:**
- Factory overhead is minimal
- Consider object pooling if creation is expensive
- Cache factory decisions if applicable

**Interview Tip:** Frequently asked: "How would you create different validators based on order type?" → Factory Method.

---

### 5. Builder Pattern (8/10) ⭐⭐⭐⭐

#### When to Use

✅ **Perfect for:**
- Complex objects with many optional parameters
- Need fluent/readable construction API
- Want validation before construction
- Step-by-step object building

**Real-World Use-Cases:**
- **Order Construction**: Many optional fields (limit price, stop loss, time in force)
- **SQL Query Building**: Fluent API (`.Select().From().Where().OrderBy()`)
- **HTTP Request Building**: Headers, body, auth, timeout
- **Configuration Objects**: Complex configs with validation
- **Report Generation**: Sections, filters, formatting options
- **Test Data Builders**: Create test objects with defaults

#### How to Use

**Structure:**
```csharp
// 1. Define builder interface
public interface IOrderBuilder {
    IOrderBuilder WithAccount(string accountId);
    IOrderBuilder WithSymbol(string symbol);
    IOrderBuilder WithQuantity(decimal quantity);
    Order Build();
    IOrderBuilder Reset();
}

// 2. Implement builder
public class OrderBuilder : IOrderBuilder {
    private string? _accountId;
    private string? _symbol;
    private decimal _quantity;
    
    public IOrderBuilder WithAccount(string accountId) {
        _accountId = accountId;
        return this; // Enables chaining
    }
    
    public Order Build() {
        // Validate required fields
        if (string.IsNullOrWhiteSpace(_accountId))
            throw new InvalidOperationException("Account ID required");
        
        // Create and return order
        return new Order(_accountId, _symbol, _quantity);
    }
}

// 3. Use builder
var order = new OrderBuilder()
    .WithAccount("ACC-001")
    .WithSymbol("AAPL")
    .WithQuantity(100)
    .Build();
```

**Key Implementation Points:**
- Methods return builder for method chaining
- `Build()` validates and creates final object
- `Reset()` clears builder for reuse
- Builder stores state until `Build()` is called

#### When NOT to Use

❌ **Don't use when:**
- Simple objects (use constructor or object initializer)
- All parameters required (use constructor)
- Object construction is trivial
- Only one representation needed
- Performance is critical (adds overhead)

**Anti-Patterns:**
- Builder for objects with 2-3 parameters
- Builder without validation
- Builder that creates invalid objects

#### Important Notes & Tips

**Enterprise Considerations:**
- Builders are typically scoped per request (transient or scoped)
- No shared state
- Safe for concurrent use if not shared
- Validation in `Build()` ensures invalid objects are never created

**Testing:**
- Test fluent interface chaining
- Test validation in `Build()`
- Test different object configurations
- Test `Reset()` functionality

**Performance:**
- Builder adds overhead (object creation)
- Consider direct construction for simple cases
- Reuse builders when possible

**Interview Tip:** This pattern is common for API design. Be ready to explain how you'd build complex objects with many optional parameters.

---

### 6. Decorator Pattern (8/10) ⭐⭐⭐⭐

#### When to Use

✅ **Perfect for:**
- Add behavior at runtime
- Cross-cutting concerns (logging, caching, retries)
- Combine behaviors flexibly
- Don't want to modify existing code

**Real-World Use-Cases:**
- **Payment Service Decorators**: Logging, metrics, retries, caching
- **API Client Decorators**: Authentication, rate limiting, circuit breakers
- **Repository Decorators**: Caching, audit logging, performance monitoring
- **Message Handler Decorators**: Retry logic, dead letter queue, tracing
- **Service Decorators**: Transaction management, security checks, validation

#### How to Use

**Structure:**
```csharp
// 1. Define component interface
public interface IPaymentService {
    Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request);
}

// 2. Implement core component
public class PaymentService : IPaymentService {
    public async Task<PaymentResult> ProcessPaymentAsync(...) {
        // Core payment logic
    }
}

// 3. Implement decorators
public class LoggingPaymentServiceDecorator : IPaymentService {
    private readonly IPaymentService _inner;
    private readonly ILogger _logger;
    
    public async Task<PaymentResult> ProcessPaymentAsync(...) {
        _logger.LogInformation("Starting payment");
        var result = await _inner.ProcessPaymentAsync(...);
        _logger.LogInformation("Payment completed");
        return result;
    }
}

// 4. Chain decorators
var core = new PaymentService(gateway, logger);
var withLogging = new LoggingPaymentServiceDecorator(core, logger);
var withMetrics = new MetricsPaymentServiceDecorator(withLogging, metrics);
var withRetry = new RetryPaymentServiceDecorator(withMetrics, logger);
```

**Key Implementation Points:**
- Decorators implement same interface as component
- Each decorator wraps inner component (composition)
- Decorators can be chained
- Execution flows: Decorator3 → Decorator2 → Decorator1 → Core

#### When NOT to Use

❌ **Don't use when:**
- Behavior should be part of core class
- Need to remove behavior (decorators are additive only)
- Simple method wrapping (use AOP frameworks like PostSharp)
- Decorators add too much overhead
- Behavior is not cross-cutting

**Anti-Patterns:**
- Decorators that change core behavior instead of adding
- Too many decorators (deep call stacks)
- Decorators with shared mutable state

#### Important Notes & Tips

**Enterprise Considerations:**
- Decorators are stateless wrappers - safe for multi-instance
- Each pod can have its own decorator chain
- Metrics decorators aggregate per-instance (use external metrics store)
- Consider decorator ordering (retry should wrap metrics)

**Testing:**
- Mock decorated service
- Test each decorator independently
- Test decorator chain composition
- Test decorator ordering

**Performance:**
- Decorators add method call overhead
- Consider AOP for high-performance scenarios
- Monitor decorator chain depth

**Interview Tip:** This pattern is essential for cross-cutting concerns. Be ready to explain how you'd add logging/metrics without modifying existing services.

---

### 7. Repository Pattern (8/10) ⭐⭐⭐⭐

#### When to Use

✅ **Perfect for:**
- Abstracting data access
- Making code testable
- Supporting multiple data sources
- Implementing Unit of Work pattern

**Real-World Use-Cases:**
- **Data Access Abstraction**: Swap in-memory for database in tests
- **Multiple Data Sources**: Database, cache, external APIs
- **Testability**: Mock repositories for unit tests
- **Transaction Management**: Unit of Work coordinates repositories
- **Query Abstraction**: Hide complex queries behind simple methods

#### How to Use

**Structure:**
```csharp
// 1. Define repository interface
public interface IRepository<TEntity, TKey> {
    Task<TEntity?> GetByIdAsync(TKey id);
    Task AddAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task DeleteAsync(TKey id);
}

// 2. Implement repository
public class InMemoryRepository<TEntity, TKey> : IRepository<TEntity, TKey> {
    private readonly ConcurrentDictionary<TKey, TEntity> _storage = new();
    
    public Task<TEntity?> GetByIdAsync(TKey id) {
        _storage.TryGetValue(id, out var entity);
        return Task.FromResult(entity);
    }
    // ... other methods
}

// 3. Unit of Work for transactions
public interface IUnitOfWork {
    Task BeginTransactionAsync();
    Task CommitAsync();
    Task RollbackAsync();
    IRepository<TEntity, TKey> GetRepository<TEntity, TKey>();
}
```

**Key Implementation Points:**
- Repository abstracts persistence details
- Unit of Work coordinates multiple repositories
- Repository implementations can be swapped (in-memory vs database)
- Repository interface enables testing

#### When NOT to Use

❌ **Don't use when:**
- Simple CRUD (use ORM directly)
- Need direct SQL everywhere
- Repository becomes pass-through (no abstraction value)
- Over-engineering for simple scenarios

**Anti-Patterns:**
- Repository that just wraps ORM without abstraction
- Repository with business logic (should be in domain)
- Repository leaky abstraction (exposes persistence details)

#### Important Notes & Tips

**Enterprise Considerations:**
- Repositories access shared database
- Use optimistic concurrency (rowversion) for multi-instance
- Unit of Work coordinates transactions
- Consider caching in repository decorator

**Testing:**
- Mock repository interface
- Use in-memory repository for integration tests
- Test Unit of Work transactions
- Test repository error handling

**Performance:**
- Repository adds abstraction layer
- Consider direct ORM access for performance-critical paths
- Cache frequently accessed entities

**Interview Tip:** This pattern is essential for testability. Be ready to explain how you'd abstract data access for testing.

---

### 8. State Pattern (8/10) ⭐⭐⭐⭐

#### When to Use

✅ **Perfect for:**
- Behavior depends on state
- Many state-based conditionals
- Well-defined state transitions
- State machine behavior

**Real-World Use-Cases:**
- **Order Lifecycles**: Pending → Placed → Filled → Cancelled
- **Workflow Engines**: State machines for business processes
- **Game Development**: Character states (idle, running, jumping)
- **Document Approval**: Draft → Review → Approved → Published
- **Payment Processing**: Pending → Processing → Completed → Failed

#### How to Use

**Structure:**
```csharp
// 1. Define state interface
public interface IOrderState {
    Task<Order> Place(Order order);
    Task<Order> Fill(Order order);
    Task<Order> Cancel(Order order);
    Task<Order> Reject(Order order);
}

// 2. Implement concrete states
public class PendingOrderState : IOrderState {
    public Task<Order> Place(Order order) {
        order.Status = OrderStatus.Placed;
        return Task.FromResult(order);
    }
    
    public Task<Order> Fill(Order order) {
        throw new InvalidOperationException("Cannot fill pending order");
    }
    // ... other methods
}

// 3. State factory
public class OrderStateFactory {
    public IOrderState CreateState(OrderStatus status) {
        return status switch {
            OrderStatus.Pending => new PendingOrderState(),
            OrderStatus.Placed => new PlacedOrderState(),
            // ...
        };
    }
}

// 4. Use state
var state = _factory.CreateState(order.Status);
await state.Place(order);
```

**Key Implementation Points:**
- Each state implements state-specific behavior
- Invalid transitions throw exceptions
- State factory creates state from enum
- Order delegates behavior to current state

#### When NOT to Use

❌ **Don't use when:**
- Simple state flags (use enum)
- Only 2-3 states (over-engineering)
- State transitions are not well-defined
- Behavior doesn't depend on state

**Anti-Patterns:**
- States with shared mutable state
- States that know about other states
- Too many state classes for simple scenarios

#### Important Notes & Tips

**Enterprise Considerations:**
- State objects are stateless
- State stored in database (not in-memory)
- Use optimistic concurrency for transitions
- Consider state machine libraries for complex scenarios

**Testing:**
- Test each state independently
- Test valid/invalid transitions
- Test state-specific behavior
- Test state factory

**Performance:**
- State objects are lightweight
- Consider caching state objects if expensive to create
- State transitions should be fast

**Interview Tip:** This pattern is common for state machines. Be ready to explain how you'd handle order lifecycle states.

---

### 9. Singleton Pattern (7/10) ⭐⭐⭐

#### When to Use

✅ **Perfect for:**
- One instance per application
- Expensive resource initialization
- Global configuration or cache
- Connection pools

**Real-World Use-Cases:**
- **Configuration Managers**: Application-wide settings
- **Connection Pools**: Database connection management
- **Caches**: Shared cache instances
- **Logging Services**: Single logger instance
- **Service Locators**: (though often anti-pattern)

#### How to Use

**Structure:**
```csharp
// Modern .NET: Use DI singleton lifetime
services.AddSingleton<IConfigurationService, ConfigurationService>();

// Access through dependency injection
public class MyService {
    private readonly IConfigurationService _config;
    
    public MyService(IConfigurationService config) {
        _config = config; // Same instance across all requests
    }
}
```

**Key Implementation Points:**
- Use DI singleton lifetime (not static singleton)
- Singleton ensures one instance per application instance
- Thread-safe access if needed
- Interface-based for testability

#### When NOT to Use

❌ **Don't use when:**
- Distributed coordination (doesn't work across pods)
- Testability concerns (use DI singleton, not static)
- Shared mutable state (causes concurrency issues)
- Over-engineering for simple scenarios

**Anti-Patterns:**
- Static singleton (hard to test)
- Singleton with mutable state
- Singleton for distributed coordination

#### Important Notes & Tips

**Enterprise Considerations:**
- ⚠️ **Critical**: Singleton is per-application instance, NOT across pods
- In Kubernetes, each pod has its own singleton instance
- **Does NOT solve distributed concurrency problems**
- Use database constraints, optimistic concurrency, or distributed locks instead

**Testing:**
- Use DI singleton (not static) for testability
- Mock `IConfigurationService` interface
- Reset DI container between tests if needed

**Performance:**
- Singleton reduces object creation overhead
- Consider thread-safety for concurrent access
- Use `Interlocked` for atomic operations

**Interview Tip:** Be ready to explain the Kubernetes caveat. Singleton does NOT guarantee uniqueness across distributed systems.

---

### 10. Abstract Factory Pattern (7/10) ⭐⭐⭐

#### When to Use

✅ **Perfect for:**
- Need families of related objects
- Objects must work together (compatibility)
- Provider ecosystems

**Real-World Use-Cases:**
- **Payment Providers**: Stripe gateway + Stripe config (compatible pair)
- **UI Toolkits**: Windows components + Windows theme
- **Database Providers**: SQL Server connection + SQL Server query builder
- **Cloud Services**: AWS services + AWS config

#### How to Use

**Structure:**
```csharp
// 1. Define factory interface
public interface IPaymentGatewayFactory {
    IPaymentGateway CreatePaymentGateway();
    GatewayConfig CreateConfiguration();
}

// 2. Implement factories
public class StripeGatewayFactory : IPaymentGatewayFactory {
    public IPaymentGateway CreatePaymentGateway() {
        return new StripeGateway();
    }
    
    public GatewayConfig CreateConfiguration() {
        return new StripeConfig(); // Compatible with StripeGateway
    }
}
```

**Key Implementation Points:**
- Factory creates multiple related products
- Products from same factory are guaranteed compatible
- Different factories create different product families
- Client uses factory interface, not concrete factories

#### When NOT to Use

❌ **Don't use when:**
- Single product creation (use Factory Method)
- Products aren't related
- Over-engineering for simple scenarios

**Anti-Patterns:**
- Mixing products from different factories
- Factory creating incompatible products
- Factory for single product type

#### Important Notes & Tips

**Enterprise Considerations:**
- Factories are stateless
- Each pod can use different factory (based on config)
- No shared state
- Factory selection based on configuration

**Testing:**
- Mock factory interface
- Test product family compatibility
- Test factory selection

**Performance:**
- Factory overhead is minimal
- Consider caching factory instances

**Interview Tip:** Be ready to explain the difference between Factory Method and Abstract Factory.

---

### 11. Adapter Pattern (7/10) ⭐⭐⭐

#### When to Use

✅ **Perfect for:**
- Integrating incompatible interfaces
- Wrapping legacy systems
- Converting sync to async
- Third-party library integration

**Real-World Use-Cases:**
- **Legacy Integration**: Old sync API → Modern async API
- **Third-Party Libraries**: Incompatible interfaces
- **Data Format Conversion**: Tuples → Objects, XML → JSON
- **Protocol Adaptation**: REST → gRPC

#### How to Use

**Structure:**
```csharp
// 1. Define target interface (what you need)
public interface IModernMarketDataProvider {
    Task<Quote> GetQuoteAsync(string symbol);
}

// 2. Define adaptee interface (what you have)
public interface ILegacyMarketDataProvider {
    (decimal price, DateTime timestamp) GetQuoteLegacy(string symbol);
}

// 3. Implement adapter
public class MarketDataAdapter : IModernMarketDataProvider {
    private readonly ILegacyMarketDataProvider _legacy;
    
    public async Task<Quote> GetQuoteAsync(string symbol) {
        var (price, timestamp) = _legacy.GetQuoteLegacy(symbol);
        return new Quote(symbol, price, timestamp);
    }
}
```

**Key Implementation Points:**
- Adapter implements target interface
- Adapter wraps adaptee (incompatible object)
- Adapter translates calls and data formats
- Client uses target interface, not adaptee

#### When NOT to Use

❌ **Don't use when:**
- Can modify source interface directly
- Simple type conversions (use mapper)
- Over-engineering for simple wrappers

**Anti-Patterns:**
- Adapter that changes behavior (should only adapt)
- Too many adapters (consider refactoring)

#### Important Notes & Tips

**Enterprise Considerations:**
- Adapters are stateless wrappers
- Safe for multi-instance
- Each pod adapts independently

**Testing:**
- Mock legacy provider
- Test adapter transformation logic
- Test async conversion

**Performance:**
- Adapter adds indirection overhead
- Consider caching if expensive

**Interview Tip:** Be ready to explain how you'd integrate a legacy sync API with modern async code.

---

### 12. Facade Pattern (7/10) ⭐⭐⭐

#### When to Use

✅ **Perfect for:**
- Simplifying complex subsystem
- Providing simple API over complex system
- Decoupling client from subsystem

**Real-World Use-Cases:**
- **API Design**: Simple interface to complex backend
- **Legacy System Wrapper**: Hide complexity of old system
- **Microservices Gateway**: Single entry point to multiple services
- **Framework APIs**: Simple interface to complex framework

#### How to Use

**Structure:**
```csharp
// 1. Define facade interface
public interface ITradingFacade {
    Task<PlaceOrderResult> PlaceOrderAsync(Order order);
}

// 2. Implement facade
public class TradingFacade : ITradingFacade {
    private readonly IOrderValidatorFactory _validatorFactory;
    private readonly ICommandHandler _commandHandler;
    private readonly IEventBus _eventBus;
    
    public async Task<PlaceOrderResult> PlaceOrderAsync(Order order) {
        // Coordinate multiple subsystems
        var validator = _validatorFactory.CreateValidator(order);
        await validator.ValidateAsync(order);
        
        var command = new PlaceOrderCommand(order);
        await _commandHandler.ExecuteAsync(command);
        
        await _eventBus.PublishAsync(new OrderPlacedEvent(order));
        
        return new PlaceOrderResult(success: true);
    }
}
```

**Key Implementation Points:**
- Facade provides simple interface
- Facade coordinates multiple subsystems
- Client only knows about facade
- Facade hides subsystem complexity

#### When NOT to Use

❌ **Don't use when:**
- Need direct subsystem access
- Subsystem already simple
- Over-engineering for simple scenarios

**Anti-Patterns:**
- Facade that becomes god object
- Facade with business logic (should delegate)

#### Important Notes & Tips

**Enterprise Considerations:**
- Facades are stateless orchestrators
- Safe for multi-instance
- Coordinates subsystem calls

**Testing:**
- Mock subsystem components
- Test facade orchestration
- Test error handling

**Performance:**
- Facade adds orchestration overhead
- Consider caching if applicable

**Interview Tip:** Be ready to explain how you'd simplify a complex subsystem API.

---

### 13. Mediator Pattern (7/10) ⭐⭐⭐

#### When to Use

✅ **Perfect for:**
- Reducing many-to-many dependencies
- Centralized communication
- CQRS implementations

**Real-World Use-Cases:**
- **Request Routing**: Route requests to handlers
- **CQRS**: Command/Query separation
- **Chat Applications**: Chat room mediates users
- **Workflow Engines**: Central coordinator

#### How to Use

**Structure:**
```csharp
// 1. Define request/response interfaces
public interface IRequest<TResponse> { }
public interface IRequestHandler<TRequest, TResponse> {
    Task<TResponse> HandleAsync(TRequest request);
}

// 2. Define mediator interface
public interface IMediator {
    Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request);
}

// 3. Implement mediator
public class Mediator : IMediator {
    private readonly IServiceProvider _serviceProvider;
    
    public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request) {
        var handlerType = typeof(IRequestHandler<,>)
            .MakeGenericType(request.GetType(), typeof(TResponse));
        var handler = _serviceProvider.GetService(handlerType);
        return await handler.HandleAsync(request);
    }
}
```

**Key Implementation Points:**
- Mediator routes requests to handlers
- Handlers implement `IRequestHandler<TRequest, TResponse>`
- Components don't know about each other
- Mediator uses reflection to find handlers

#### When NOT to Use

❌ **Don't use when:**
- Simple one-to-one communication
- Only 2-3 components
- Over-engineering for simple scenarios

**Anti-Patterns:**
- Mediator that becomes god object
- Handlers that know about each other

#### Important Notes & Tips

**Enterprise Considerations:**
- Mediators are stateless routers
- Safe for multi-instance
- Routes requests based on type

**Testing:**
- Mock mediator
- Test handler resolution
- Test request routing

**Performance:**
- Reflection adds overhead
- Consider code generation for performance
- Cache handler lookups

**Interview Tip:** Be ready to explain how you'd reduce coupling between components.

---

### 14. Chain of Responsibility Pattern (7/10) ⭐⭐⭐

#### When to Use

✅ **Perfect for:**
- Processing pipelines
- Multiple handlers can process request
- Dynamic handler selection

**Real-World Use-Cases:**
- **Validation Pipelines**: Basic → Risk → Account validation
- **Middleware**: ASP.NET Core middleware chain
- **Exception Handling**: Try multiple handlers
- **Request Processing**: Authentication → Authorization → Validation

#### How to Use

**Structure:**
```csharp
// 1. Define handler interface
public interface IValidationHandler {
    Task<ValidationResult> HandleAsync(Order order);
}

// 2. Base handler with chain behavior
public abstract class BaseValidationHandler : IValidationHandler {
    protected readonly IValidationHandler? _next;
    
    public async Task<ValidationResult> HandleAsync(Order order) {
        var result = await ValidateAsync(order);
        if (!result.IsValid) return result;
        
        return _next != null 
            ? await _next.HandleAsync(order)
            : new ValidationResult(isValid: true);
    }
    
    protected abstract Task<ValidationResult> ValidateAsync(Order order);
}

// 3. Concrete handlers
public class BasicValidationHandler : BaseValidationHandler { ... }
public class RiskValidationHandler : BaseValidationHandler { ... }

// 4. Chain handlers
var chain = new BasicValidationHandler(
    new RiskValidationHandler(
        new AccountValidationHandler(null)
    )
);
```

**Key Implementation Points:**
- Handlers linked in chain
- Each handler processes or passes to next
- Request flows through chain
- Chain can be dynamic

#### When NOT to Use

❌ **Don't use when:**
- Request must go to specific handler
- Simple sequential processing
- Over-engineering for simple scenarios

**Anti-Patterns:**
- Chain that hides where request is handled
- Handlers with side effects
- Circular chains

#### Important Notes & Tips

**Enterprise Considerations:**
- Chain handlers are stateless
- Safe for multi-instance
- Request flows through chain

**Testing:**
- Test each handler independently
- Test chain flow
- Test handler ordering

**Performance:**
- Chain adds method call overhead
- Consider short-circuiting if applicable

**Interview Tip:** Be ready to explain how you'd implement a validation pipeline.

---

### 15. Strategy Advanced Pattern (10/10) ⭐⭐⭐⭐⭐

#### When to Use

✅ **Perfect for:**
- Runtime provider selection based on user input
- Dynamic strategy selection from external configuration
- Provider ecosystems with resolver mechanism

**Real-World Use-Cases:**
- **Payment Provider Selection**: User chooses Stripe/PayPal/Crypto
- **Cloud Provider Selection**: AWS/Azure/GCP based on config
- **Algorithm Selection**: User selects optimization strategy
- **Service Provider Ecosystems**: Multiple providers with dynamic selection

#### How to Use

**Structure:**
```csharp
// 1. Define provider interface
public interface IPaymentProvider {
    string ProviderKey { get; }
    ProviderInfo Info { get; }
    Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request);
}

// 2. Implement providers
public class StripePaymentProvider : IPaymentProvider { ... }
public class PayPalPaymentProvider : IPaymentProvider { ... }

// 3. Define resolver
public interface IPaymentProviderResolver {
    IPaymentProvider? ResolveProvider(string providerKey);
    IEnumerable<IPaymentProvider> GetAvailableProviders();
}

// 4. Implement resolver
public class PaymentProviderResolver : IPaymentProviderResolver {
    private readonly IReadOnlyDictionary<string, IPaymentProvider> _providers;
    
    public IPaymentProvider? ResolveProvider(string providerKey) {
        _providers.TryGetValue(providerKey, out var provider);
        return provider;
    }
}

// 5. Use in service
public class PaymentService {
    public async Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request) {
        var provider = _resolver.ResolveProvider(request.ProviderKey);
        return await provider.ProcessPaymentAsync(request);
    }
}
```

**Key Implementation Points:**
- Resolver provides O(1) lookup by key
- Provider selection based on user input/configuration
- Resolver discovers all registered providers dynamically
- Extensible: Add new provider without modifying existing code

#### When NOT to Use

❌ **Don't use when:**
- Strategy selection based on data (use regular Strategy)
- Only one provider
- Selection is compile-time only

**Anti-Patterns:**
- Resolver that doesn't cache providers
- Providers with shared mutable state

#### Important Notes & Tips

**Enterprise Considerations:**
- Providers are singleton (stateless)
- Resolver caches providers for O(1) lookup
- Safe for multi-instance deployment
- Provider selection based on request data

**Testing:**
- Test resolver with different provider keys
- Test provider discovery
- Test provider selection logic

**Performance:**
- Resolver provides O(1) lookup
- Consider caching provider info

**Interview Tip:** This extends Strategy pattern. Be ready to explain how you'd select providers at runtime based on user input.

---

### 16. Prototype Pattern (5/10) ⭐⭐

#### When to Use

✅ **Perfect for:**
- Expensive object creation
- Creating objects similar to existing ones
- Snapshots for rollback

**Real-World Use-Cases:**
- **Snapshots**: Order snapshots for backtesting
- **Templates**: Clone template objects
- **Expensive Creation**: Clone instead of creating from scratch
- **Undo/Redo**: Clone state for undo functionality

#### How to Use

**Structure:**
```csharp
// 1. Define prototype interface
public interface IPrototype<T> {
    T Clone();
}

// 2. Implement prototype
public class OrderSnapshot : IPrototype<OrderSnapshot> {
    private readonly Order _order;
    private readonly Dictionary<string, string> _metadata;
    
    public OrderSnapshot Clone() {
        // Deep copy
        return new OrderSnapshot(
            new Order(_order.OrderId, _order.Symbol, ...),
            new Dictionary<string, string>(_metadata)
        );
    }
}
```

**Key Implementation Points:**
- Prototypes implement `Clone()` method
- Clone creates independent copy (deep copy)
- Clones are independent - modifications don't affect originals
- Useful for expensive object creation

#### When NOT to Use

❌ **Don't use when:**
- Simple object creation
- Objects cheap to create
- Shallow copy is sufficient
- Over-engineering for simple scenarios

**Anti-Patterns:**
- Shallow copy when deep copy needed
- Clone that shares references

#### Important Notes & Tips

**Enterprise Considerations:**
- Prototypes cloned per-instance
- No shared state
- Deep cloning ensures independence

**Testing:**
- Test clone creates independent copy
- Test modifications don't affect original
- Test deep vs shallow cloning

**Performance:**
- Deep cloning can be expensive
- Consider serialization for complex objects
- Cache clones if applicable

**Interview Tip:** Less common pattern. Be ready to explain when you'd use cloning vs creating new objects.

---

## Quick Decision Matrix

| Need | Pattern | Score |
|------|---------|-------|
| Switch algorithms at runtime | Strategy | 10/10 |
| Notify multiple components | Observer | 9/10 |
| Undo/redo functionality | Command | 9/10 |
| Create objects based on conditions | Factory Method | 8/10 |
| Build complex objects step-by-step | Builder | 8/10 |
| Add behavior without modifying code | Decorator | 8/10 |
| Abstract data access | Repository | 8/10 |
| State-specific behavior | State | 8/10 |
| One instance per app | Singleton | 7/10 |
| Compatible object families | Abstract Factory | 7/10 |
| Integrate incompatible interfaces | Adapter | 7/10 |
| Simplify complex subsystem | Facade | 7/10 |
| Reduce component coupling | Mediator | 7/10 |
| Processing pipeline | Chain of Responsibility | 7/10 |
| Clone expensive objects | Prototype | 5/10 |
| Dynamic provider selection | Strategy Advanced | 10/10 |

---

## Common Pitfalls and Anti-Patterns

### 1. Over-Engineering
**Problem:** Using patterns for simple scenarios  
**Solution:** Start simple, add patterns when complexity justifies it

### 2. Pattern Confusion
**Problem:** Mixing up similar patterns (Factory vs Abstract Factory, Strategy vs State)  
**Solution:** Understand the core difference: Factory creates objects, Strategy selects algorithms

### 3. Tight Coupling
**Problem:** Patterns that increase coupling instead of reducing it  
**Solution:** Use interfaces, dependency injection, and proper abstractions

### 4. Ignoring Testing
**Problem:** Patterns that make code harder to test  
**Solution:** Design patterns should improve testability, not hinder it

### 5. Distributed Systems Misconceptions
**Problem:** Assuming Singleton works across pods  
**Solution:** Understand that Singleton is per-process, not per-cluster

### 6. Performance Overhead
**Problem:** Patterns that add unnecessary overhead  
**Solution:** Measure performance, optimize when needed

---

## Conclusion

Design patterns are tools to solve common problems. The key is:
1. **Understand the problem** before choosing a pattern
2. **Start simple** and add patterns when complexity justifies
3. **Know when NOT to use** a pattern (as important as when to use)
4. **Test your implementation** to ensure patterns improve code quality
5. **Consider enterprise concerns** (scalability, testability, maintainability)

Master these 16 patterns, and you'll be equipped to handle most enterprise software challenges.

---

*This guide is based on real-world implementations in the Design Patterns Playground API project.*  
*For class diagrams, see [DesignPatternsClassDiagrams.md](./DesignPatternsClassDiagrams.md)*  
*For quick reference, see [DesignPatternsGuide.md](./DesignPatternsGuide.md)*
