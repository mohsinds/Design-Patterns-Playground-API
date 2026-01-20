# Design Patterns Guide - Interview & Enterprise Reference

## Table of Contents
1. [Pattern Comparison Table](#pattern-comparison-table)
2. [Pattern Selection Cheat-Sheet](#pattern-selection-cheat-sheet)
3. [Rapid-Fire Interview Drills](#rapid-fire-interview-drills)
4. [Pattern Details](#pattern-details)

---

## Pattern Comparison Table

| Pattern | Purpose | Best Use-Case | Key Tradeoff | Importance Score |
|---------|---------|---------------|-------------|------------------|
| **Singleton** | Ensure one instance per app | Configuration, connection pools, caches | Not for distributed coordination | 7/10 |
| **Factory Method** | Encapsulate object creation | Conditional object creation based on input | Adds abstraction layer | 8/10 |
| **Abstract Factory** | Create families of related objects | Provider ecosystems (Stripe vs PayPal) | More complex than Factory Method | 7/10 |
| **Builder** | Construct complex objects step-by-step | Objects with many optional parameters | More code than constructors | 8/10 |
| **Adapter** | Make incompatible interfaces work together | Legacy system integration | Adds indirection layer | 7/10 |
| **Command** | Encapsulate requests as objects | Undo/redo, queuing, audit trails | More objects than direct calls | 9/10 |
| **Decorator** | Add behavior dynamically | Cross-cutting concerns (logging, retries) | Can create deep call stacks | 8/10 |
| **Strategy** | Encapsulate algorithms, make them interchangeable | Runtime algorithm selection | More classes than conditionals | 10/10 |
| **Observer** | One-to-many dependency, notify on state change | Event-driven architecture, pub-sub | Can cause memory leaks if not unsubscribed | 9/10 |
| **Facade** | Simplify complex subsystem | API design, legacy system wrapper | Hides subsystem details | 7/10 |
| **Repository** | Abstract data access | Testability, multiple data sources | Can become pass-through | 8/10 |
| **Mediator** | Reduce many-to-many dependencies | Request routing, CQRS | Can become god object | 7/10 |
| **State** | Encapsulate state-specific behavior | State machines, lifecycles | More classes than enums | 8/10 |
| **Prototype** | Clone objects instead of creating new | Expensive creation, snapshots | Deep copying complexity | 5/10 |
| **Chain of Responsibility** | Pass request through handler chain | Validation pipelines, middleware | Can hide where request is handled | 7/10 |

---

## Pattern Selection Cheat-Sheet

### Decision Rules

#### **When to use Singleton?**
- ✅ Need one instance per application
- ✅ Expensive resource initialization
- ✅ Global configuration or cache
- ❌ NOT for distributed coordination
- ❌ NOT for testability (use DI)

#### **When to use Factory Method?**
- ✅ Object creation depends on input/conditions
- ✅ Want to decouple creation from usage
- ✅ Multiple product types with same interface
- ❌ Simple object creation (use constructor)
- ❌ Only one product type

#### **When to use Abstract Factory?**
- ✅ Need families of related objects
- ✅ Objects must work together (compatibility)
- ✅ Provider ecosystems (payment, cloud, database)
- ❌ Single product creation
- ❌ Products aren't related

#### **When to use Builder?**
- ✅ Complex objects with many optional parameters
- ✅ Need fluent/readable construction API
- ✅ Want validation before construction
- ❌ Simple objects (use constructor)
- ❌ All parameters required

#### **When to use Adapter?**
- ✅ Integrating incompatible interfaces
- ✅ Wrapping legacy systems
- ✅ Converting sync to async
- ❌ Can modify source interface directly
- ❌ Simple type conversions

#### **When to use Command?**
- ✅ Need undo/redo functionality
- ✅ Want to queue operations
- ✅ Need audit trail
- ✅ Transaction support
- ❌ Simple method calls
- ❌ No undo/queue needed

#### **When to use Decorator?**
- ✅ Add behavior at runtime
- ✅ Cross-cutting concerns (logging, caching)
- ✅ Want to combine behaviors flexibly
- ❌ Behavior should be in core class
- ❌ Need to remove behavior

#### **When to use Strategy?**
- ✅ Multiple algorithms for same task
- ✅ Switch algorithms at runtime
- ✅ Avoid conditional statements
- ❌ Simple conditional logic
- ❌ Only one algorithm

#### **When to use Observer?**
- ✅ Notify multiple objects about changes
- ✅ Event-driven architecture
- ✅ Loose coupling needed
- ❌ One-to-one notifications
- ❌ Order of notifications critical

#### **When to use Facade?**
- ✅ Simplify complex subsystem
- ✅ Provide simple API over complex system
- ✅ Decouple client from subsystem
- ❌ Need direct subsystem access
- ❌ Subsystem already simple

#### **When to use Repository?**
- ✅ Abstract data access
- ✅ Make code testable
- ✅ Support multiple data sources
- ❌ Simple CRUD (use ORM directly)
- ❌ Need direct SQL everywhere

#### **When to use Mediator?**
- ✅ Many-to-many dependencies
- ✅ Reduce coupling between components
- ✅ Centralized communication
- ❌ Simple one-to-one communication
- ❌ Only 2-3 components

#### **When to use State?**
- ✅ Behavior depends on state
- ✅ Many state-based conditionals
- ✅ Well-defined state transitions
- ❌ Simple state flags (use enum)
- ❌ Only 2-3 states

#### **When to use Prototype?**
- ✅ Expensive object creation
- ✅ Create objects similar to existing
- ✅ Need snapshots
- ❌ Simple object creation
- ❌ Objects cheap to create

#### **When to use Chain of Responsibility?**
- ✅ Multiple handlers can process request
- ✅ Processing pipeline needed
- ✅ Dynamic handler selection
- ❌ Request must go to specific handler
- ❌ Simple sequential processing

---

## Rapid-Fire Questions

### Scenario → Pattern → One-Liner

1. **"How to ensure only one configuration manager exists?"**
   → **Singleton** - One instance per application for shared configuration

2. **"Design a system that creates validators based on order type"**
   → **Factory Method** - Encapsulate validator creation logic based on order characteristics

3. **"Create compatible payment gateway and its configuration together"**
   → **Abstract Factory** - Create families of related objects (gateway + config)

4. **"Build complex orders with many optional fields step-by-step"**
   → **Builder** - Fluent interface for constructing complex objects

5. **"Make legacy synchronous API work with async code"**
   → **Adapter** - Wrap incompatible interface to match expected interface

6. **"Implement undo/redo for trading operations"**
   → **Command** - Encapsulate operations as objects with undo capability

7. **"Add logging and retry logic to payment service without modifying it"**
   → **Decorator** - Dynamically add cross-cutting concerns

8. **"Switch between different pricing algorithms at runtime"**
   → **Strategy** - Encapsulate algorithms and make them interchangeable

9. **"Notify multiple components when order is placed"**
   → **Observer** - One-to-many dependency for event notifications

10. **"Simplify complex trading subsystem with single API"**
    → **Facade** - Unified interface to complex subsystem

11. **"Make data access testable by abstracting database"**
    → **Repository** - Abstract data access layer for testability

12. **"Route requests to appropriate handlers without coupling"**
    → **Mediator** - Centralized communication hub

13. **"Handle order lifecycle states (Pending → Placed → Filled)"**
    → **State** - Encapsulate state-specific behavior and transitions

14. **"Clone portfolio snapshots for backtesting"**
    → **Prototype** - Clone expensive objects instead of creating new

15. **"Process requests through validation pipeline (Basic → Risk → Account)"**
    → **Chain of Responsibility** - Pass request through handler chain

16. **"Design a connection pool shared across application"**
    → **Singleton** - One instance for expensive resource management

17. **"Create different report formats (PDF, Excel, CSV) based on request"**
    → **Factory Method** - Create objects based on type parameter

18. **"Support multiple cloud providers (AWS vs Azure) with compatible services"**
    → **Abstract Factory** - Create provider-specific service families

19. **"Queue order operations for later processing"**
    → **Command** - Encapsulate operations for queuing and execution

20. **"Add caching layer to repository without modifying repository code"**
    → **Decorator** - Wrap repository with caching behavior

---

## Pattern Details

### 1. Singleton (7/10)

**Definition:** Ensures a class has only one instance per application instance and provides global access.

**How it works:**
- DI container creates one instance (AddSingleton)
- Instance shared across all requests in same application
- Thread-safe access using Interlocked for counters
- Each Kubernetes pod has its own singleton instance

**Code Explanation:**
```csharp
// ConfigurationService uses Interlocked.Increment for thread-safe instance counting
// InstanceId tracks which instance this is (useful for demo)
// AccessCount increments atomically for thread-safe tracking
// Dictionary stores configuration (read-only after construction)
```

**Multi-Instance Behavior:**
- Each pod = separate singleton instance
- NOT for distributed coordination
- Use database constraints or distributed locks instead

**Testing:**
- Use DI singleton (not static) for testability
- Mock IConfigurationService interface
- Reset DI container between tests

---

### 2. Factory Method (8/10)

**Definition:** Defines interface for creating objects, lets subclasses decide which class to instantiate.

**How it works:**
- Factory interface defines creation method
- Concrete factory implements creation logic
- Factory decides which product to create based on input
- Products implement common interface

**Code Explanation:**
```csharp
// OrderValidatorFactory.CreateValidator() examines order value
// If orderValue >= 100000, creates LargeOrderValidator
// Otherwise creates StandardOrderValidator
// Factory encapsulates decision logic, easy to extend with new validators
```

**Multi-Instance Behavior:**
- Factory is stateless, safe for multi-instance
- Each pod creates validators independently
- No shared state between factory instances

**Testing:**
- Mock IOrderValidatorFactory
- Test factory selection logic
- Test created validators independently

---

### 3. Abstract Factory (7/10)

**Definition:** Provides interface for creating families of related objects without specifying concrete classes.

**How it works:**
- Factory creates multiple related products
- Products work together (compatibility requirement)
- Different factories create different product families
- Client uses factory interface, not concrete factories

**Code Explanation:**
```csharp
// StripeGatewayFactory creates StripeGateway + StripeConfig (compatible family)
// PayPalGatewayFactory creates PayPalGateway + PayPalConfig (compatible family)
// Both implement IPaymentGatewayFactory interface
// Products from same factory are guaranteed to work together
```

**Multi-Instance Behavior:**
- Factories are stateless
- Each pod can use different factory (based on config)
- No shared state

**Testing:**
- Mock IPaymentGatewayFactory
- Test product family compatibility
- Test factory selection

---

### 4. Builder (8/10)

**Definition:** Separates construction of complex object from representation, allows step-by-step construction.

**How it works:**
- Builder provides fluent interface (method chaining)
- Each method sets one aspect of object
- Build() validates and creates final object
- Reset() clears builder for reuse

**Code Explanation:**
```csharp
// OrderBuilder.WithAccount().WithSymbol().WithSide().WithQuantity().WithPrice().Build()
// Each method returns builder for chaining
// Build() validates required fields before creating Order
// Reset() clears builder state for reuse
```

**Multi-Instance Behavior:**
- Builders are typically scoped per request
- No shared state
- Safe for concurrent use (if not shared)

**Testing:**
- Test fluent interface chaining
- Test validation in Build()
- Test different object configurations

---

### 5. Adapter (7/10)

**Definition:** Allows incompatible interfaces to work together by wrapping object with adapter.

**How it works:**
- Adapter implements target interface
- Wraps adaptee (incompatible object)
- Translates calls from target to adaptee format
- Converts data formats (sync to async, tuples to objects)

**Code Explanation:**
```csharp
// MarketDataAdapter implements IModernMarketDataProvider (async)
// Wraps ILegacyMarketDataProvider (sync, tuple-based)
// GetQuoteAsync() calls legacy.GetQuoteLegacy() and converts:
// - Tuple to Quote object
// - Sync to async (Task.Run)
// - Legacy timestamp format to DateTime
```

**Multi-Instance Behavior:**
- Adapters are stateless wrappers
- Safe for multi-instance
- Each pod adapts independently

**Testing:**
- Mock legacy provider
- Test adapter transformation logic
- Test async conversion

---

### 6. Command (9/10)

**Definition:** Encapsulates request as object, enabling parameterization, queuing, logging, undo.

**How it works:**
- Command interface defines Execute() and Undo()
- Concrete commands encapsulate operations
- Command handler manages execution (retry, audit, queue)
- Commands can be queued, logged, undone

**Code Explanation:**
```csharp
// PlaceOrderCommand encapsulates order placement operation
// ExecuteAsync() places order, stores original state for undo
// UndoAsync() restores original state or removes order
// CommandHandler wraps execution with:
// - Retry logic (3 attempts with exponential backoff)
// - Audit logging (all commands logged)
// - Queue support (ConcurrentQueue for async processing)
```

**Multi-Instance Behavior:**
- Commands queued in distributed message queue (Kafka)
- Command IDs must be globally unique (UUID)
- Handlers should be idempotent
- Use outbox pattern for reliable delivery

**Testing:**
- Test command execution
- Test undo functionality
- Test command queuing
- Mock command handlers

---

### 7. Decorator (8/10)

**Definition:** Attaches additional responsibilities to objects dynamically without modifying structure.

**How it works:**
- Decorator implements same interface as component
- Wraps component (composition)
- Adds behavior before/after component call
- Decorators can be chained (decorator wraps decorator)

**Code Explanation:**
```csharp
// Decorator chain: RetryPaymentServiceDecorator → MetricsPaymentServiceDecorator → LoggingPaymentServiceDecorator → PaymentService
// Each decorator wraps inner service
// Logging decorator logs before/after
// Metrics decorator records duration and counters
// Retry decorator retries on failure
// All decorators implement IPaymentService interface
```

**Multi-Instance Behavior:**
- Decorators are stateless wrappers
- Safe for multi-instance
- Metrics aggregate per-instance (use external store)

**Testing:**
- Mock decorated service
- Test each decorator independently
- Test decorator chain composition

---

### 8. Strategy (10/10)

**Definition:** Defines family of algorithms, encapsulates each, makes them interchangeable.

**How it works:**
- Strategy interface defines algorithm
- Concrete strategies implement different algorithms
- Context uses strategy interface (not concrete)
- Strategy selected at runtime based on data

**Code Explanation:**
```csharp
// IPricingStrategy defines CalculatePrice() method
// MarketPriceStrategy, LimitPriceStrategy, VwapPricingStrategy, RiskAdjustedPricingStrategy implement it
// PricingStrategySelector.SelectStrategy() chooses strategy based on order value
// PaymentService uses selected strategy to calculate price
// Strategies are interchangeable - can switch at runtime
```

**Multi-Instance Behavior:**
- Strategies are stateless
- Safe for multi-instance
- Selection based on request data

**Testing:**
- Test each strategy independently
- Test strategy selection logic
- Test strategy interchangeability

---

### 9. Observer (9/10)

**Definition:** Defines one-to-many dependency, when subject changes, all observers notified.

**How it works:**
- Subject maintains list of observers
- Subject notifies observers on state change
- Observers implement handler interface
- Loose coupling (subject doesn't know observer details)

**Code Explanation:**
```csharp
// IEventBus manages subscriptions (ConcurrentDictionary<Type, List<handlers>>)
// PublishAsync() notifies all handlers for event type
// Handlers subscribe via Subscribe() method
// Event handlers process events (OrderPlacedEventHandler, OrderFilledEventHandler, etc.)
// Also publishes to Kafka for distributed pub-sub
```

**Multi-Instance Behavior:**
- Use distributed message broker (Kafka, RabbitMQ)
- Each pod publishes/subscribes to events
- Event handlers must be idempotent
- Use outbox pattern for reliable publishing

**Testing:**
- Subscribe test handlers
- Publish events, verify handlers called
- Test event ordering

---

### 10. Facade (7/10)

**Definition:** Provides unified interface to set of interfaces in subsystem, hiding complexity.

**How it works:**
- Facade wraps multiple subsystem components
- Provides simple interface to complex operations
- Coordinates calls to multiple components
- Hides subsystem complexity from client

**Code Explanation:**
```csharp
// TradingFacade.PlaceOrderAsync() coordinates:
// 1. Creates order
// 2. Validates via IOrderValidatorFactory
// 3. Persists via ICommandHandler (PlaceOrderCommand)
// 4. Publishes event via IEventBus
// Client only calls facade, doesn't know about subsystems
```

**Multi-Instance Behavior:**
- Facades are stateless orchestrators
- Safe for multi-instance
- Coordinates subsystem calls

**Testing:**
- Mock subsystem components
- Test facade orchestration
- Test error handling

---

### 11. Repository (8/10)

**Definition:** Mediates between domain and data mapping, abstracts data access.

**How it works:**
- Repository interface abstracts CRUD operations
- Implementation handles persistence details
- Unit of Work coordinates multiple repositories
- Enables testing with mock repositories

**Code Explanation:**
```csharp
// IRepository<TEntity, TKey> provides GetByIdAsync, AddAsync, UpdateAsync, DeleteAsync
// InMemoryRepository uses ConcurrentDictionary for thread-safe storage
// InMemoryUnitOfWork coordinates transactions (BeginTransaction, Commit, Rollback)
// Repository abstracts persistence - can swap in-memory for database
```

**Multi-Instance Behavior:**
- Repositories access shared database
- Use optimistic concurrency (rowversion)
- Unit of Work coordinates transactions

**Testing:**
- Mock repository interface
- Use in-memory repository for integration tests
- Test Unit of Work transactions

---

### 12. Mediator (7/10)

**Definition:** Encapsulates how objects interact, promotes loose coupling.

**How it works:**
- Mediator routes requests to handlers
- Components don't know about each other
- Mediator knows all components
- Reduces many-to-many dependencies

**Code Explanation:**
```csharp
// IMediator.SendAsync<TResponse>(IRequest<TResponse>) routes requests
// Mediator uses reflection to find IRequestHandler<TRequest, TResponse>
// Resolves handler from DI container
// Handler processes request and returns response
// Components only know mediator, not each other
```

**Multi-Instance Behavior:**
- Mediators are stateless routers
- Safe for multi-instance
- Routes requests based on type

**Testing:**
- Mock mediator
- Test handler resolution
- Test request routing

---

### 13. State (8/10)

**Definition:** Allows object to alter behavior when internal state changes.

**How it works:**
- State interface defines state behavior
- Concrete states implement state-specific behavior
- Context delegates to current state
- State transitions create new state objects

**Code Explanation:**
```csharp
// IOrderState defines Place(), Fill(), Cancel(), Reject() methods
// PendingOrderState, PlacedOrderState, FilledOrderState, CancelledOrderState implement it
// Each state validates transitions (throws if invalid)
// OrderStateFactory.CreateState() creates state from OrderStatus enum
// State pattern prevents invalid transitions at compile time
```

**Multi-Instance Behavior:**
- State objects are stateless
- State stored in database (not in-memory)
- Use optimistic concurrency for transitions

**Testing:**
- Test each state independently
- Test valid/invalid transitions
- Test state-specific behavior

---

### 14. Prototype (5/10)

**Definition:** Creates objects by cloning existing instances instead of creating new.

**How it works:**
- Prototype interface defines Clone() method
- Concrete prototypes implement deep cloning
- Clone creates independent copy
- Useful for expensive object creation

**Code Explanation:**
```csharp
// OrderSnapshot.Clone() creates deep copy:
// - Clones Order (all properties copied)
// - Clones Metadata dictionary (new dictionary with copied values)
// - Returns new OrderSnapshot with independent state
// PortfolioSnapshot.Clone() clones list of OrderSnapshots and positions
```

**Multi-Instance Behavior:**
- Prototypes cloned per-instance
- No shared state
- Deep cloning ensures independence

**Testing:**
- Test clone creates independent copy
- Test modifications don't affect original
- Test deep vs shallow cloning

---

### 15. Chain of Responsibility (7/10)

**Definition:** Passes request along chain of handlers, each decides to process or pass on.

**How it works:**
- Handler interface defines Handle() method
- Handlers linked in chain
- Request passed to first handler
- Handler processes or passes to next

**Code Explanation:**
```csharp
// BaseValidationHandler.HandleAsync() validates, then calls _next.HandleAsync() if valid
// BasicValidationHandler validates quantity, price, symbol
// RiskValidationHandler validates order value limits
// AccountValidationHandler validates account exists
// Chain: Basic → Risk → Account (each validates and passes if valid)
```

**Multi-Instance Behavior:**
- Chain handlers are stateless
- Safe for multi-instance
- Request flows through chain

**Testing:**
- Test each handler independently
- Test chain flow
- Test handler ordering

---

## Quick Reference: Importance Scores

1. **Strategy (10/10)** - Most important, extremely common
2. **Observer (9/10)** - Event-driven architecture, very common
3. **Command (9/10)** - Undo/redo, queuing, very common
4. **Factory Method (8/10)** - Object creation, very common
5. **Builder (8/10)** - Complex construction, very common
6. **Decorator (8/10)** - Cross-cutting concerns, very common
7. **Repository (8/10)** - Data access, very common
8. **State (8/10)** - State machines, very common
9. **Singleton (7/10)** - Common but often misunderstood
10. **Abstract Factory (7/10)** - Provider ecosystems, common
11. **Adapter (7/10)** - Legacy integration, common
12. **Facade (7/10)** - API design, common
13. **Mediator (7/10)** - CQRS, common
14. **Chain of Responsibility (7/10)** - Middleware, common
15. **Prototype (5/10)** - Less common, specific use cases

---

## Interview Tips

1. **Always explain tradeoffs** - No pattern is perfect
2. **Mention alternatives** - Show you understand when NOT to use
3. **Discuss testing** - Interviewers care about testability
4. **Explain multi-instance behavior** - Critical for distributed systems
5. **Give real examples** - FinTech examples show practical knowledge
6. **Start with problem** - Explain what problem pattern solves
7. **Show code structure** - Draw or explain class relationships
8. **Mention related patterns** - Factory vs Abstract Factory, Strategy vs State

---

*Last Updated: 2026*  
*For interview preparation and enterprise reference*  
*By Mohsin Rasheed ([LinkedIn](https://linkedin.com/in/mohsinrasheed))*

