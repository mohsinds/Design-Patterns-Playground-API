# Design Patterns Class Diagrams

*Complete UML class diagrams for all 16 design patterns implemented in this project*

---

## Table of Contents

1. [Singleton Pattern](#1-singleton-pattern)
2. [Factory Method Pattern](#2-factory-method-pattern)
3. [Abstract Factory Pattern](#3-abstract-factory-pattern)
4. [Builder Pattern](#4-builder-pattern)
5. [Adapter Pattern](#5-adapter-pattern)
6. [Command Pattern](#6-command-pattern)
7. [Decorator Pattern](#7-decorator-pattern)
8. [Strategy Pattern](#8-strategy-pattern)
9. [Observer Pattern](#9-observer-pattern)
10. [Facade Pattern](#10-facade-pattern)
11. [Repository Pattern](#11-repository-pattern)
12. [Mediator Pattern](#12-mediator-pattern)
13. [State Pattern](#13-state-pattern)
14. [Prototype Pattern](#14-prototype-pattern)
15. [Chain of Responsibility Pattern](#15-chain-of-responsibility-pattern)
16. [Strategy Advanced Pattern](#16-strategy-advanced-pattern)

---

## 1. Singleton Pattern

```mermaid
classDiagram
    class IConfigurationService {
        <<interface>>
        +string InstanceId
        +int AccessCount
        +Dictionary~string, string~ Configuration
        +string GetConfiguration(string key)
        +void IncrementAccessCount()
    }
    
    class ConfigurationService {
        -static int _instanceCount
        -string _instanceId
        -int _accessCount
        -Dictionary~string, string~ _configuration
        +string InstanceId
        +int AccessCount
        +Dictionary~string, string~ Configuration
        +string GetConfiguration(string key)
        +void IncrementAccessCount()
    }
    
    class SingletonScenario {
        -IConfigurationService _service
        +PatternDemoResponse RunDemo()
        +PatternTestResponse RunTest()
    }
    
    IConfigurationService <|.. ConfigurationService
    SingletonScenario --> IConfigurationService : uses
```

**Key Relationships:**
- `ConfigurationService` implements `IConfigurationService`
- `SingletonScenario` depends on `IConfigurationService`
- DI container creates single instance via `AddSingleton<>()`

---

## 2. Factory Method Pattern

```mermaid
classDiagram
    class IOrderValidator {
        <<interface>>
        +bool Validate(Order order)
    }
    
    class IOrderValidatorFactory {
        <<interface>>
        +IOrderValidator CreateValidator(Order order)
    }
    
    class StandardOrderValidator {
        +bool Validate(Order order)
    }
    
    class LargeOrderValidator {
        +bool Validate(Order order)
    }
    
    class OrderValidatorFactory {
        -decimal LargeOrderThreshold
        +IOrderValidator CreateValidator(Order order)
    }
    
    class FactoryMethodScenario {
        -IOrderValidatorFactory _factory
        +PatternDemoResponse RunDemo()
        +PatternTestResponse RunTest()
    }
    
    IOrderValidator <|.. StandardOrderValidator
    IOrderValidator <|.. LargeOrderValidator
    IOrderValidatorFactory <|.. OrderValidatorFactory
    OrderValidatorFactory --> IOrderValidator : creates
    FactoryMethodScenario --> IOrderValidatorFactory : uses
```

**Key Relationships:**
- `OrderValidatorFactory` implements `IOrderValidatorFactory`
- Factory creates different validators based on order characteristics
- All validators implement `IOrderValidator` interface

---

## 3. Abstract Factory Pattern

```mermaid
classDiagram
    class IPaymentGateway {
        <<interface>>
        +string ProviderName
        +Task~PaymentResult~ ProcessPaymentAsync(PaymentRequest request)
    }
    
    class IPaymentGatewayFactory {
        <<interface>>
        +string FactoryType
        +IPaymentGateway CreatePaymentGateway()
        +GatewayConfig CreateConfiguration()
    }
    
    class FakeStripeGateway {
        +string ProviderName
        +Task~PaymentResult~ ProcessPaymentAsync(PaymentRequest request)
    }
    
    class FakePayPalGateway {
        +string ProviderName
        +Task~PaymentResult~ ProcessPaymentAsync(PaymentRequest request)
    }
    
    class StripeGatewayFactory {
        +string FactoryType
        +IPaymentGateway CreatePaymentGateway()
        +GatewayConfig CreateConfiguration()
    }
    
    class PayPalGatewayFactory {
        +string FactoryType
        +IPaymentGateway CreatePaymentGateway()
        +GatewayConfig CreateConfiguration()
    }
    
    class GatewayConfig {
        +string ProviderName
        +Dictionary~string, string~ Settings
    }
    
    class AbstractFactoryScenario {
        -IPaymentGatewayFactory _stripeFactory
        -IPaymentGatewayFactory _payPalFactory
        +PatternDemoResponse RunDemo()
        +PatternTestResponse RunTest()
    }
    
    IPaymentGateway <|.. FakeStripeGateway
    IPaymentGateway <|.. FakePayPalGateway
    IPaymentGatewayFactory <|.. StripeGatewayFactory
    IPaymentGatewayFactory <|.. PayPalGatewayFactory
    StripeGatewayFactory --> FakeStripeGateway : creates
    StripeGatewayFactory --> GatewayConfig : creates
    PayPalGatewayFactory --> FakePayPalGateway : creates
    PayPalGatewayFactory --> GatewayConfig : creates
    AbstractFactoryScenario --> IPaymentGatewayFactory : uses
```

**Key Relationships:**
- Each factory creates a family of related objects (gateway + config)
- `StripeGatewayFactory` creates `FakeStripeGateway` and `StripeConfig`
- `PayPalGatewayFactory` creates `FakePayPalGateway` and `PayPalConfig`
- Products from same factory are guaranteed compatible

---

## 4. Builder Pattern

```mermaid
classDiagram
    class IOrderBuilder {
        <<interface>>
        +IOrderBuilder WithAccount(string accountId)
        +IOrderBuilder WithSymbol(string symbol)
        +IOrderBuilder WithSide(OrderSide side)
        +IOrderBuilder WithQuantity(decimal quantity)
        +IOrderBuilder WithPrice(decimal price)
        +IOrderBuilder WithLimitPrice(decimal? limitPrice)
        +Order Build()
        +IOrderBuilder Reset()
    }
    
    class OrderBuilder {
        -string _accountId
        -string _symbol
        -OrderSide _side
        -decimal _quantity
        -decimal _price
        -decimal? _limitPrice
        +IOrderBuilder WithAccount(string accountId)
        +IOrderBuilder WithSymbol(string symbol)
        +IOrderBuilder WithSide(OrderSide side)
        +IOrderBuilder WithQuantity(decimal quantity)
        +IOrderBuilder WithPrice(decimal price)
        +IOrderBuilder WithLimitPrice(decimal? limitPrice)
        +Order Build()
        +IOrderBuilder Reset()
    }
    
    class Order {
        +string OrderId
        +string AccountId
        +string Symbol
        +OrderSide Side
        +decimal Quantity
        +decimal Price
    }
    
    class BuilderScenario {
        -IOrderBuilder _builder
        +PatternDemoResponse RunDemo()
        +PatternTestResponse RunTest()
    }
    
    IOrderBuilder <|.. OrderBuilder
    OrderBuilder --> Order : creates
    BuilderScenario --> IOrderBuilder : uses
```

**Key Relationships:**
- `OrderBuilder` implements `IOrderBuilder` for fluent interface
- Methods return `IOrderBuilder` for method chaining
- `Build()` validates and creates final `Order` object
- `Reset()` clears builder state for reuse

---

## 5. Adapter Pattern

```mermaid
classDiagram
    class IModernMarketDataProvider {
        <<interface>>
        +Task~Quote~ GetQuoteAsync(string symbol)
    }
    
    class ILegacyMarketDataProvider {
        <<interface>>
        +(decimal, DateTime) GetQuoteLegacy(string symbol)
    }
    
    class MarketDataAdapter {
        -ILegacyMarketDataProvider _legacy
        +Task~Quote~ GetQuoteAsync(string symbol)
        -Quote ConvertToQuote((decimal, DateTime) legacyQuote)
    }
    
    class LegacyMarketDataProvider {
        +(decimal, DateTime) GetQuoteLegacy(string symbol)
    }
    
    class Quote {
        +string Symbol
        +decimal Bid
        +decimal Ask
        +decimal Last
        +DateTime Timestamp
    }
    
    class AdapterScenario {
        -IModernMarketDataProvider _adapter
        +PatternDemoResponse RunDemo()
        +PatternTestResponse RunTest()
    }
    
    IModernMarketDataProvider <|.. MarketDataAdapter
    ILegacyMarketDataProvider <|.. LegacyMarketDataProvider
    MarketDataAdapter --> ILegacyMarketDataProvider : wraps
    MarketDataAdapter --> Quote : creates
    AdapterScenario --> IModernMarketDataProvider : uses
```

**Key Relationships:**
- `MarketDataAdapter` implements modern interface `IModernMarketDataProvider`
- Adapter wraps legacy `ILegacyMarketDataProvider`
- Converts sync methods to async, tuples to objects
- Client uses modern interface without knowing about legacy system

---

## 6. Command Pattern

```mermaid
classDiagram
    class ICommand {
        <<interface>>
        +string CommandId
        +bool SupportsUndo
        +Task~CommandResult~ ExecuteAsync()
        +Task~CommandResult~ UndoAsync()
    }
    
    class ICommandHandler {
        <<interface>>
        +Task~CommandResult~ ExecuteAsync(ICommand command)
        +Task QueueAsync(ICommand command)
        +List~CommandAuditEntry~ GetAuditLog()
        +int GetQueueCount()
    }
    
    class PlaceOrderCommand {
        -Order _order
        -IOrderRepository _repository
        -Order _originalOrder
        +string CommandId
        +bool SupportsUndo
        +Task~CommandResult~ ExecuteAsync()
        +Task~CommandResult~ UndoAsync()
    }
    
    class CommandHandler {
        -ConcurrentQueue~ICommand~ _commandQueue
        -List~CommandAuditEntry~ _auditLog
        +Task~CommandResult~ ExecuteAsync(ICommand command)
        +Task QueueAsync(ICommand command)
        +List~CommandAuditEntry~ GetAuditLog()
        +int GetQueueCount()
        -void AuditCommand(ICommand command, string action)
    }
    
    class IOrderRepository {
        <<interface>>
        +Task~Order~ GetByIdAsync(string orderId)
        +Task AddAsync(Order order)
        +Task UpdateAsync(Order order)
        +Task DeleteAsync(string orderId)
    }
    
    class CommandResult {
        +bool Success
        +string ErrorMessage
        +object Data
    }
    
    class CommandScenario {
        -ICommandHandler _handler
        -IOrderRepository _repository
        +PatternDemoResponse RunDemo()
        +PatternTestResponse RunTest()
    }
    
    ICommand <|.. PlaceOrderCommand
    ICommandHandler <|.. CommandHandler
    PlaceOrderCommand --> IOrderRepository : uses
    CommandHandler --> ICommand : executes
    CommandScenario --> ICommandHandler : uses
    CommandScenario --> IOrderRepository : uses
```

**Key Relationships:**
- `PlaceOrderCommand` implements `ICommand` and encapsulates order placement
- `CommandHandler` executes commands with retry, audit, and queue support
- Commands can be executed or queued for later processing
- Commands support undo by storing original state

---

## 7. Decorator Pattern

```mermaid
classDiagram
    class IPaymentService {
        <<interface>>
        +Task~PaymentResult~ ProcessPaymentAsync(PaymentRequest request)
    }
    
    class PaymentService {
        -IPaymentGateway _gateway
        +Task~PaymentResult~ ProcessPaymentAsync(PaymentRequest request)
    }
    
    class LoggingPaymentServiceDecorator {
        -IPaymentService _inner
        -ILogger _logger
        +Task~PaymentResult~ ProcessPaymentAsync(PaymentRequest request)
    }
    
    class MetricsPaymentServiceDecorator {
        -IPaymentService _inner
        -IMetrics _metrics
        +Task~PaymentResult~ ProcessPaymentAsync(PaymentRequest request)
    }
    
    class RetryPaymentServiceDecorator {
        -IPaymentService _inner
        -ILogger _logger
        +Task~PaymentResult~ ProcessPaymentAsync(PaymentRequest request)
    }
    
    class PaymentRequest {
        +string TransactionId
        +decimal Amount
        +string Currency
    }
    
    class PaymentResult {
        +bool Success
        +string TransactionId
    }
    
    class DecoratorScenario {
        -IPaymentService _decoratedService
        +PatternDemoResponse RunDemo()
        +PatternTestResponse RunTest()
    }
    
    IPaymentService <|.. PaymentService
    IPaymentService <|.. LoggingPaymentServiceDecorator
    IPaymentService <|.. MetricsPaymentServiceDecorator
    IPaymentService <|.. RetryPaymentServiceDecorator
    LoggingPaymentServiceDecorator --> IPaymentService : wraps
    MetricsPaymentServiceDecorator --> IPaymentService : wraps
    RetryPaymentServiceDecorator --> IPaymentService : wraps
    PaymentService --> IPaymentGateway : uses
    DecoratorScenario --> IPaymentService : uses
```

**Key Relationships:**
- All decorators implement `IPaymentService` interface
- Each decorator wraps inner `IPaymentService` (composition)
- Decorators can be chained: `Retry → Metrics → Logging → Core`
- Each decorator adds behavior before/after calling inner service

---

## 8. Strategy Pattern

```mermaid
classDiagram
    class IPricingStrategy {
        <<interface>>
        +string StrategyName
        +decimal CalculatePrice(Order order, Quote marketQuote)
    }
    
    class IPricingStrategySelector {
        <<interface>>
        +IPricingStrategy SelectStrategy(Order order)
    }
    
    class MarketPriceStrategy {
        +string StrategyName
        +decimal CalculatePrice(Order order, Quote marketQuote)
    }
    
    class LimitPriceStrategy {
        +string StrategyName
        +decimal CalculatePrice(Order order, Quote marketQuote)
    }
    
    class VwapPricingStrategy {
        +string StrategyName
        +decimal CalculatePrice(Order order, Quote marketQuote)
    }
    
    class RiskAdjustedPricingStrategy {
        -decimal _riskPremium
        +string StrategyName
        +decimal CalculatePrice(Order order, Quote marketQuote)
    }
    
    class PricingStrategySelector {
        -IEnumerable~IPricingStrategy~ _strategies
        +IPricingStrategy SelectStrategy(Order order)
    }
    
    class StrategyScenario {
        -IPricingStrategySelector _selector
        -IEnumerable~IPricingStrategy~ _strategies
        +PatternDemoResponse RunDemo()
        +PatternTestResponse RunTest()
    }
    
    IPricingStrategy <|.. MarketPriceStrategy
    IPricingStrategy <|.. LimitPriceStrategy
    IPricingStrategy <|.. VwapPricingStrategy
    IPricingStrategy <|.. RiskAdjustedPricingStrategy
    IPricingStrategySelector <|.. PricingStrategySelector
    PricingStrategySelector --> IPricingStrategy : selects
    StrategyScenario --> IPricingStrategySelector : uses
    StrategyScenario --> IPricingStrategy : uses
```

**Key Relationships:**
- All strategies implement `IPricingStrategy` interface
- `PricingStrategySelector` selects appropriate strategy based on order
- Strategies are interchangeable - can switch at runtime
- Client uses strategy interface, not concrete implementations

---

## 9. Observer Pattern

```mermaid
classDiagram
    class IDomainEvent {
        <<interface>>
        +string EventId
        +string EventType
        +DateTime OccurredAt
    }
    
    class IEventBus {
        <<interface>>
        +Task PublishAsync~TEvent~(TEvent event)
        +void Subscribe~TEvent~(IEventHandler~TEvent~ handler)
    }
    
    class IEventHandler~TEvent~ {
        <<interface>>
        +Task HandleAsync(TEvent event)
    }
    
    class OrderPlacedEvent {
        +string EventId
        +string EventType
        +DateTime OccurredAt
        +Order Order
    }
    
    class OrderFilledEvent {
        +string EventId
        +string EventType
        +DateTime OccurredAt
        +Order Order
    }
    
    class InMemoryEventBus {
        -ConcurrentDictionary~Type, List~object~~ _handlers
        -IKafkaProducer _kafkaProducer
        +Task PublishAsync~TEvent~(TEvent event)
        +void Subscribe~TEvent~(IEventHandler~TEvent~ handler)
    }
    
    class OrderPlacedEventHandler {
        +Task HandleAsync(OrderPlacedEvent event)
    }
    
    class OrderFilledEventHandler {
        +Task HandleAsync(OrderFilledEvent event)
    }
    
    class ObserverScenario {
        -IEventBus _eventBus
        +PatternDemoResponse RunDemo()
        +PatternTestResponse RunTest()
    }
    
    IDomainEvent <|.. OrderPlacedEvent
    IDomainEvent <|.. OrderFilledEvent
    IEventBus <|.. InMemoryEventBus
    IEventHandler~OrderPlacedEvent~ <|.. OrderPlacedEventHandler
    IEventHandler~OrderFilledEvent~ <|.. OrderFilledEventHandler
    InMemoryEventBus --> IEventHandler : notifies
    InMemoryEventBus --> IKafkaProducer : publishes
    ObserverScenario --> IEventBus : uses
```

**Key Relationships:**
- `IEventBus` manages subscriptions and publishing
- Event handlers subscribe to specific event types
- When event is published, all subscribed handlers are notified
- Handlers implement `IEventHandler<TEvent>` for specific event types
- Event bus also publishes to Kafka for distributed pub-sub

---

## 10. Facade Pattern

```mermaid
classDiagram
    class ITradingFacade {
        <<interface>>
        +Task~PlaceOrderResult~ PlaceOrderAsync(Order order)
    }
    
    class TradingFacade {
        -IOrderValidatorFactory _validatorFactory
        -ICommandHandler _commandHandler
        -IEventBus _eventBus
        +Task~PlaceOrderResult~ PlaceOrderAsync(Order order)
    }
    
    class IOrderValidatorFactory {
        <<interface>>
        +IOrderValidator CreateValidator(Order order)
    }
    
    class ICommandHandler {
        <<interface>>
        +Task~CommandResult~ ExecuteAsync(ICommand command)
    }
    
    class IEventBus {
        <<interface>>
        +Task PublishAsync~TEvent~(TEvent event)
    }
    
    class PlaceOrderResult {
        +bool Success
        +string OrderId
        +string Message
    }
    
    class FacadeScenario {
        -ITradingFacade _facade
        +PatternDemoResponse RunDemo()
        +PatternTestResponse RunTest()
    }
    
    ITradingFacade <|.. TradingFacade
    TradingFacade --> IOrderValidatorFactory : uses
    TradingFacade --> ICommandHandler : uses
    TradingFacade --> IEventBus : uses
    FacadeScenario --> ITradingFacade : uses
```

**Key Relationships:**
- `TradingFacade` provides simple interface to complex subsystem
- Facade coordinates multiple subsystems (validator, command handler, event bus)
- Client only knows about facade, not subsystems
- Facade hides complexity and simplifies API

---

## 11. Repository Pattern

```mermaid
classDiagram
    class IRepository~TEntity, TKey~ {
        <<interface>>
        +Task~TEntity~ GetByIdAsync(TKey id)
        +Task AddAsync(TEntity entity)
        +Task UpdateAsync(TEntity entity)
        +Task DeleteAsync(TKey id)
    }
    
    class IUnitOfWork {
        <<interface>>
        +Task BeginTransactionAsync()
        +Task CommitAsync()
        +Task RollbackAsync()
        +IRepository~TEntity, TKey~ GetRepository~TEntity, TKey~()
    }
    
    class InMemoryRepository~TEntity, TKey~ {
        -ConcurrentDictionary~TKey, TEntity~ _storage
        +Task~TEntity~ GetByIdAsync(TKey id)
        +Task AddAsync(TEntity entity)
        +Task UpdateAsync(TEntity entity)
        +Task DeleteAsync(TKey id)
    }
    
    class InMemoryUnitOfWork {
        -Dictionary~Type, object~ _repositories
        +Task BeginTransactionAsync()
        +Task CommitAsync()
        +Task RollbackAsync()
        +IRepository~TEntity, TKey~ GetRepository~TEntity, TKey~()
    }
    
    class RepositoryScenario {
        -IUnitOfWork _unitOfWork
        +PatternDemoResponse RunDemo()
        +PatternTestResponse RunTest()
    }
    
    IRepository~TEntity, TKey~ <|.. InMemoryRepository~TEntity, TKey~
    IUnitOfWork <|.. InMemoryUnitOfWork
    InMemoryUnitOfWork --> IRepository : manages
    RepositoryScenario --> IUnitOfWork : uses
```

**Key Relationships:**
- `IRepository<TEntity, TKey>` abstracts data access operations
- `IUnitOfWork` coordinates multiple repositories and transactions
- Repository implementations handle persistence details
- Unit of Work ensures transactional consistency across repositories

---

## 12. Mediator Pattern

```mermaid
classDiagram
    class IRequest~TResponse~ {
        <<interface>>
    }
    
    class IRequestHandler~TRequest, TResponse~ {
        <<interface>>
        +Task~TResponse~ HandleAsync(TRequest request)
    }
    
    class IMediator {
        <<interface>>
        +Task~TResponse~ SendAsync~TResponse~(IRequest~TResponse~ request)
    }
    
    class PlaceOrderRequest {
        +Order Order
    }
    
    class PlaceOrderResponse {
        +bool Success
        +string OrderId
    }
    
    class GetOrderRequest {
        +string OrderId
    }
    
    class GetOrderResponse {
        +Order Order
    }
    
    class Mediator {
        -IServiceProvider _serviceProvider
        +Task~TResponse~ SendAsync~TResponse~(IRequest~TResponse~ request)
    }
    
    class PlaceOrderRequestHandler {
        -IOrderRepository _repository
        +Task~PlaceOrderResponse~ HandleAsync(PlaceOrderRequest request)
    }
    
    class GetOrderRequestHandler {
        -IOrderRepository _repository
        +Task~GetOrderResponse~ HandleAsync(GetOrderRequest request)
    }
    
    class MediatorScenario {
        -IMediator _mediator
        +PatternDemoResponse RunDemo()
        +PatternTestResponse RunTest()
    }
    
    IRequest~PlaceOrderResponse~ <|.. PlaceOrderRequest
    IRequest~GetOrderResponse~ <|.. GetOrderRequest
    IRequestHandler~PlaceOrderRequest, PlaceOrderResponse~ <|.. PlaceOrderRequestHandler
    IRequestHandler~GetOrderRequest, GetOrderResponse~ <|.. GetOrderRequestHandler
    IMediator <|.. Mediator
    Mediator --> IRequestHandler : routes to
    MediatorScenario --> IMediator : uses
```

**Key Relationships:**
- `IMediator` routes requests to appropriate handlers
- Handlers implement `IRequestHandler<TRequest, TResponse>`
- Components don't know about each other - only mediator
- Mediator uses reflection to find and resolve handlers from DI

---

## 13. State Pattern

```mermaid
classDiagram
    class IOrderState {
        <<interface>>
        +Task~Order~ Place(Order order)
        +Task~Order~ Fill(Order order)
        +Task~Order~ Cancel(Order order)
        +Task~Order~ Reject(Order order)
    }
    
    class OrderStateFactory {
        +IOrderState CreateState(OrderStatus status)
    }
    
    class PendingOrderState {
        +Task~Order~ Place(Order order)
        +Task~Order~ Fill(Order order)
        +Task~Order~ Cancel(Order order)
        +Task~Order~ Reject(Order order)
    }
    
    class PlacedOrderState {
        +Task~Order~ Place(Order order)
        +Task~Order~ Fill(Order order)
        +Task~Order~ Cancel(Order order)
        +Task~Order~ Reject(Order order)
    }
    
    class FilledOrderState {
        +Task~Order~ Place(Order order)
        +Task~Order~ Fill(Order order)
        +Task~Order~ Cancel(Order order)
        +Task~Order~ Reject(Order order)
    }
    
    class CancelledOrderState {
        +Task~Order~ Place(Order order)
        +Task~Order~ Fill(Order order)
        +Task~Order~ Cancel(Order order)
        +Task~Order~ Reject(Order order)
    }
    
    class Order {
        +string OrderId
        +OrderStatus Status
    }
    
    class StateScenario {
        -OrderStateFactory _factory
        +PatternDemoResponse RunDemo()
        +PatternTestResponse RunTest()
    }
    
    IOrderState <|.. PendingOrderState
    IOrderState <|.. PlacedOrderState
    IOrderState <|.. FilledOrderState
    IOrderState <|.. CancelledOrderState
    OrderStateFactory --> IOrderState : creates
    StateScenario --> OrderStateFactory : uses
    StateScenario --> IOrderState : uses
```

**Key Relationships:**
- Each state implements `IOrderState` with state-specific behavior
- `OrderStateFactory` creates state objects from `OrderStatus` enum
- Invalid transitions throw exceptions (enforced at compile time)
- Order delegates behavior to current state object

---

## 14. Prototype Pattern

```mermaid
classDiagram
    class IPrototype~T~ {
        <<interface>>
        +T Clone()
    }
    
    class OrderSnapshot {
        -Order _order
        -Dictionary~string, string~ _metadata
        +Order Order
        +Dictionary~string, string~ Metadata
        +OrderSnapshot Clone()
    }
    
    class PortfolioSnapshot {
        -List~OrderSnapshot~ _orderSnapshots
        -Dictionary~string, decimal~ _positions
        +List~OrderSnapshot~ OrderSnapshots
        +Dictionary~string, decimal~ Positions
        +PortfolioSnapshot Clone()
    }
    
    class Order {
        +string OrderId
        +string Symbol
        +decimal Quantity
        +decimal Price
    }
    
    class PrototypeScenario {
        +PatternDemoResponse RunDemo()
        +PatternTestResponse RunTest()
    }
    
    IPrototype~OrderSnapshot~ <|.. OrderSnapshot
    IPrototype~PortfolioSnapshot~ <|.. PortfolioSnapshot
    OrderSnapshot --> Order : contains
    PortfolioSnapshot --> OrderSnapshot : contains
    PrototypeScenario --> OrderSnapshot : uses
    PrototypeScenario --> PortfolioSnapshot : uses
```

**Key Relationships:**
- Prototypes implement `Clone()` method for deep copying
- `OrderSnapshot` clones order and metadata independently
- `PortfolioSnapshot` clones list of order snapshots and positions
- Clones are independent - modifications don't affect originals

---

## 15. Chain of Responsibility Pattern

```mermaid
classDiagram
    class IValidationHandler {
        <<interface>>
        +Task~ValidationResult~ HandleAsync(Order order)
    }
    
    class BaseValidationHandler {
        #IValidationHandler _next
        +BaseValidationHandler(IValidationHandler next)
        +Task~ValidationResult~ HandleAsync(Order order)
        #abstract Task~ValidationResult~ ValidateAsync(Order order)
    }
    
    class BasicValidationHandler {
        +Task~ValidationResult~ HandleAsync(Order order)
        #Task~ValidationResult~ ValidateAsync(Order order)
    }
    
    class RiskValidationHandler {
        +Task~ValidationResult~ HandleAsync(Order order)
        #Task~ValidationResult~ ValidateAsync(Order order)
    }
    
    class AccountValidationHandler {
        +Task~ValidationResult~ HandleAsync(Order order)
        #Task~ValidationResult~ ValidateAsync(Order order)
    }
    
    class ValidationResult {
        +bool IsValid
        +string ErrorMessage
    }
    
    class ChainOfResponsibilityScenario {
        -IValidationHandler _handlerChain
        +PatternDemoResponse RunDemo()
        +PatternTestResponse RunTest()
    }
    
    IValidationHandler <|.. BaseValidationHandler
    BaseValidationHandler <|-- BasicValidationHandler
    BaseValidationHandler <|-- RiskValidationHandler
    BaseValidationHandler <|-- AccountValidationHandler
    BaseValidationHandler --> IValidationHandler : chains to next
    ChainOfResponsibilityScenario --> IValidationHandler : uses
```

**Key Relationships:**
- `BaseValidationHandler` implements chain behavior
- Handlers are linked together (each has reference to next)
- Request flows through chain: Basic → Risk → Account
- Each handler validates and passes to next if valid

---

## 16. Strategy Advanced Pattern

```mermaid
classDiagram
    class IPaymentProvider {
        <<interface>>
        +string ProviderKey
        +ProviderInfo Info
        +Task~PaymentResult~ ProcessPaymentAsync(PaymentRequest request)
    }
    
    class IPaymentProviderResolver {
        <<interface>>
        +IPaymentProvider ResolveProvider(string providerKey)
        +IEnumerable~IPaymentProvider~ GetAvailableProviders()
    }
    
    class IPaymentService {
        <<interface>>
        +Task~PaymentResult~ ProcessPaymentAsync(PaymentRequest request)
        +IEnumerable~ProviderInfo~ GetAvailableProviders()
    }
    
    class StripePaymentProvider {
        +string ProviderKey
        +ProviderInfo Info
        +Task~PaymentResult~ ProcessPaymentAsync(PaymentRequest request)
    }
    
    class PayPalPaymentProvider {
        +string ProviderKey
        +ProviderInfo Info
        +Task~PaymentResult~ ProcessPaymentAsync(PaymentRequest request)
    }
    
    class CryptoPaymentProvider {
        +string ProviderKey
        +ProviderInfo Info
        +Task~PaymentResult~ ProcessPaymentAsync(PaymentRequest request)
    }
    
    class PaymentProviderResolver {
        -IReadOnlyDictionary~string, IPaymentProvider~ _providers
        +IPaymentProvider ResolveProvider(string providerKey)
        +IEnumerable~IPaymentProvider~ GetAvailableProviders()
    }
    
    class PaymentService {
        -IPaymentProviderResolver _resolver
        +Task~PaymentResult~ ProcessPaymentAsync(PaymentRequest request)
        +IEnumerable~ProviderInfo~ GetAvailableProviders()
    }
    
    class PaymentRequest {
        +decimal Amount
        +string Currency
        +string ProviderKey
        +string CustomerEmail
    }
    
    class PaymentResult {
        +string TransactionId
        +string Status
        +string ProviderUsed
        +DateTime ProcessedAt
    }
    
    class ProviderInfo {
        +string Key
        +string Name
        +decimal MinimumAmount
        +string[] SupportedCurrencies
    }
    
    class StrategyAdvancedController {
        -IPaymentService _paymentService
        +Task~IActionResult~ ProcessPayment(PaymentRequest request)
        +IActionResult GetProviders()
    }
    
    IPaymentProvider <|.. StripePaymentProvider
    IPaymentProvider <|.. PayPalPaymentProvider
    IPaymentProvider <|.. CryptoPaymentProvider
    IPaymentProviderResolver <|.. PaymentProviderResolver
    IPaymentService <|.. PaymentService
    PaymentProviderResolver --> IPaymentProvider : resolves
    PaymentService --> IPaymentProviderResolver : uses
    StrategyAdvancedController --> IPaymentService : uses
```

**Key Relationships:**
- All providers implement `IPaymentProvider` interface
- `PaymentProviderResolver` provides O(1) lookup by provider key
- `PaymentService` uses resolver to select provider at runtime
- Provider selection based on user input (`ProviderKey` in request)
- Resolver discovers all registered providers dynamically

---

## Legend

**Interface:** `<<interface>>`  
**Abstract Class:** `<<abstract>>` or `#abstract`  
**Composition:** `-->` (arrow with solid line)  
**Inheritance:** `<|--` (arrow with triangle)  
**Implementation:** `<|..` (dashed arrow with triangle)  
**Dependency:** `-->` (arrow with dashed line)

---

## Diagram Conventions

1. **Interfaces** are marked with `<<interface>>` stereotype
2. **Abstract classes** use `#` for protected members
3. **Composition** (has-a) uses solid arrow `-->`
4. **Inheritance** (is-a) uses triangle arrow `<|--`
5. **Implementation** uses dashed triangle `<|..`
6. **Generics** use `~` notation: `List~T~`, `IRepository~TEntity, TKey~`
7. **Collections** are shown with type parameters

---

## Using These Diagrams

These diagrams can be:
- Rendered in Markdown viewers that support Mermaid (GitHub, GitLab, VS Code)
- Exported to PNG/SVG using Mermaid Live Editor
- Used as reference for understanding pattern structure
- Shared with team members for code reviews
- Included in documentation and presentations

**Mermaid Live Editor:** https://mermaid.live/

---

*All diagrams are based on the actual implementations in this project*
*Last Updated: 2026*  
*For interview preparation and enterprise reference*  
*By Mohsin Rasheed ([LinkedIn](https://linkedin.com/in/mohsinrasheed))*

