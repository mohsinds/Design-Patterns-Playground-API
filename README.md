# Design Patterns Playground API

A comprehensive ASP.NET Core Web API project demonstrating **16 design patterns** in a FinTech/trading systems context. This project serves as a teaching repository for understanding design patterns with realistic, production-grade implementations.

## Why This Project is Essential for Learning Design Patterns

### 🎯 **Practical, Production-Ready Examples**

Unlike theoretical tutorials, this project demonstrates design patterns in **real-world enterprise scenarios**:
- **FinTech/Trading Context**: Order management, payment processing, risk calculations
- **Production-Grade Code**: Thread-safe implementations, dependency injection, proper error handling
- **Distributed Systems Awareness**: Notes on Kubernetes, multi-instance deployments, and scalability

### 📚 **Complete Learning Resources**

This repository provides everything you need to master design patterns:

1. **Working Code Examples**: 16 fully implemented patterns with HTTP endpoints
2. **Class Diagrams**: Visual UML diagrams for each pattern (see [`DesignPatternsClassDiagrams.md`](./DesignPatternsClassDiagrams.md))
3. **Comprehensive Guide**: Detailed documentation on when to use, how to use, and when NOT to use each pattern (see [`DesignPatternsCompleteGuide.md`](./DesignPatternsCompleteGuide.md))
4. **Interview Preparation**: Importance scores, use-cases, and common interview questions
5. **Interactive Testing**: Run `/demo` and `/test` endpoints to see patterns in action

### 🏆 **What Makes This Different**

- **Realistic Scenarios**: Patterns solve actual problems (pricing algorithms, order validation, payment processing)
- **Modern .NET Practices**: Uses .NET 9.0, async/await, dependency injection, modern C# features
- **Enterprise Considerations**: Thread-safety, distributed systems, observability (logging, metrics)
- **Testable Design**: Each pattern demonstrates how to write testable, maintainable code
- **Visual Learning**: Mermaid class diagrams show relationships and structure

### 📖 **Documentation Structure**

- **[DesignPatternsClassDiagrams.md](./DesignPatternsClassDiagrams.md)**: UML class diagrams for all 16 patterns
- **[DesignPatternsCompleteGuide.md](./DesignPatternsCompleteGuide.md)**: Comprehensive guide with use-cases, implementation tips, and importance scores
- **[DesignPatternsGuide.md](./DesignPatternsGuide.md)**: Quick reference with comparison tables and interview drills

### 🎓 **Perfect For**

- **Software Engineers** preparing for system design interviews
- **Developers** refactoring legacy codebases
- **Students** learning object-oriented design principles
- **Teams** establishing coding standards and patterns
- **Architects** designing scalable systems

## Overview

This API exposes separate HTTP endpoints for each design pattern, allowing you to:
- **Demo**: See the pattern in action with clear JSON responses
- **Test**: Run deterministic test scenarios that return PASS/FAIL results

## Prerequisites

- .NET 9.0 SDK or later
- Your favorite HTTP client (curl, Postman, etc.)

## Getting Started

1. **Restore dependencies and build:**
   ```bash
   dotnet restore
   dotnet build
   ```

2. **Run the application:**
   ```bash
   dotnet run
   ```

3. **Access the API:**
   - API Base URL: `https://localhost:5001` or `http://localhost:5000`
   - OpenAPI/Swagger: `https://localhost:5001/openapi/v1.json` (in Development mode)

## Design Patterns Implemented

### 1. Singleton
**Route:** `/api/patterns/singleton`

Demonstrates singleton pattern with important notes about distributed systems. Shows why singleton does NOT solve distributed concurrency in Kubernetes.

**Endpoints:**
- `GET /api/patterns/singleton/demo` - Demonstrates singleton behavior
- `GET /api/patterns/singleton/test` - Runs singleton tests

**Example:**
```bash
curl https://localhost:5001/api/patterns/singleton/demo
curl https://localhost:5001/api/patterns/singleton/test
```

### 2. Factory Method
**Route:** `/api/patterns/factory-method`

Creates different order validators based on order characteristics (standard vs. large orders).

**Endpoints:**
- `GET /api/patterns/factory-method/demo`
- `GET /api/patterns/factory-method/test`

**Example:**
```bash
curl https://localhost:5001/api/patterns/factory-method/demo
curl https://localhost:5001/api/patterns/factory-method/test
```

### 3. Abstract Factory
**Route:** `/api/patterns/abstract-factory`

Creates families of related objects (payment gateway + configuration) for different providers (Stripe, PayPal).

**Endpoints:**
- `GET /api/patterns/abstract-factory/demo`
- `GET /api/patterns/abstract-factory/test`

**Example:**
```bash
curl https://localhost:5001/api/patterns/abstract-factory/demo
curl https://localhost:5001/api/patterns/abstract-factory/test
```

### 4. Builder
**Route:** `/api/patterns/builder`

Provides a fluent interface for constructing complex Order objects step-by-step.

**Endpoints:**
- `GET /api/patterns/builder/demo`
- `GET /api/patterns/builder/test`

**Example:**
```bash
curl https://localhost:5001/api/patterns/builder/demo
curl https://localhost:5001/api/patterns/builder/test
```

### 5. Adapter
**Route:** `/api/patterns/adapter`

Adapts legacy market data provider interface to modern async interface.

**Endpoints:**
- `GET /api/patterns/adapter/demo`
- `GET /api/patterns/adapter/test`

**Example:**
```bash
curl https://localhost:5001/api/patterns/adapter/demo
curl https://localhost:5001/api/patterns/adapter/test
```

### 6. Command
**Route:** `/api/patterns/command`

Encapsulates requests as objects with retry, queue, audit, and undo support.

**Endpoints:**
- `GET /api/patterns/command/demo`
- `GET /api/patterns/command/test`

**Example:**
```bash
curl https://localhost:5001/api/patterns/command/demo
curl https://localhost:5001/api/patterns/command/test
```

### 7. Decorator
**Route:** `/api/patterns/decorator`

Adds cross-cutting concerns (logging, metrics, retries) dynamically without modifying core service.

**Endpoints:**
- `GET /api/patterns/decorator/demo`
- `GET /api/patterns/decorator/test`

**Example:**
```bash
curl https://localhost:5001/api/patterns/decorator/demo
curl https://localhost:5001/api/patterns/decorator/test
```

### 8. Strategy
**Route:** `/api/patterns/strategy`

Different pricing algorithms (Market, Limit, VWAP, Risk-Adjusted) selected at runtime.

**Endpoints:**
- `GET /api/patterns/strategy/demo`
- `GET /api/patterns/strategy/test`

**Example:**
```bash
curl https://localhost:5001/api/patterns/strategy/demo
curl https://localhost:5001/api/patterns/strategy/test
```

### 9. Observer / Pub-Sub
**Route:** `/api/patterns/observer`

Domain events published and handled by multiple subscribers. Includes Kafka integration mention.

**Endpoints:**
- `GET /api/patterns/observer/demo`
- `GET /api/patterns/observer/test`

**Example:**
```bash
curl https://localhost:5001/api/patterns/observer/demo
curl https://localhost:5001/api/patterns/observer/test
```

### 10. Facade
**Route:** `/api/patterns/facade`

Simplifies complex trading subsystem (validation, risk, repository, events) behind simple interface.

**Endpoints:**
- `GET /api/patterns/facade/demo`
- `GET /api/patterns/facade/test`

**Example:**
```bash
curl https://localhost:5001/api/patterns/facade/demo
curl https://localhost:5001/api/patterns/facade/test
```

### 11. Repository
**Route:** `/api/patterns/repository`

Abstracts data access, enables testing. Includes Unit of Work pattern for transaction coordination.

**Endpoints:**
- `GET /api/patterns/repository/demo`
- `GET /api/patterns/repository/test`

**Example:**
```bash
curl https://localhost:5001/api/patterns/repository/demo
curl https://localhost:5001/api/patterns/repository/test
```

### 12. Mediator
**Route:** `/api/patterns/mediator`

Routes requests to handlers, reducing many-to-many dependencies between components.

**Endpoints:**
- `GET /api/patterns/mediator/demo`
- `GET /api/patterns/mediator/test`

**Example:**
```bash
curl https://localhost:5001/api/patterns/mediator/demo
curl https://localhost:5001/api/patterns/mediator/test
```

### 13. State
**Route:** `/api/patterns/state`

Encapsulates order lifecycle state transitions (Pending -> Placed -> Filled/Cancelled) with validation.

**Endpoints:**
- `GET /api/patterns/state/demo`
- `GET /api/patterns/state/test`

**Example:**
```bash
curl https://localhost:5001/api/patterns/state/demo
curl https://localhost:5001/api/patterns/state/test
```

### 14. Prototype
**Route:** `/api/patterns/prototype`

Creates deep copies of objects for snapshots, backtests, and cloning scenarios.

**Endpoints:**
- `GET /api/patterns/prototype/demo`
- `GET /api/patterns/prototype/test`

**Example:**
```bash
curl https://localhost:5001/api/patterns/prototype/demo
curl https://localhost:5001/api/patterns/prototype/test
```

### 15. Chain of Responsibility
**Route:** `/api/patterns/chain-of-responsibility`

Validation pipeline where each handler processes or passes to next (Basic -> Risk -> Account validation).

**Endpoints:**
- `GET /api/patterns/chain-of-responsibility/demo`
- `GET /api/patterns/chain-of-responsibility/test`

**Example:**
```bash
curl https://localhost:5001/api/patterns/chain-of-responsibility/demo
curl https://localhost:5001/api/patterns/chain-of-responsibility/test
```

### 16. Strategy Advanced
**Route:** `/api/strategy-advanced`

Advanced strategy pattern with resolver/factory for dynamic payment provider selection at runtime.

**Endpoints:**
- `POST /api/strategy-advanced/process-payment` - Process payment with selected provider
- `GET /api/strategy-advanced/providers` - Get available payment providers

**Example:**
```bash
curl -X POST https://localhost:5001/api/strategy-advanced/process-payment \
  -H "Content-Type: application/json" \
  -d '{"amount": 100, "currency": "USD", "providerKey": "stripe", "customerEmail": "test@example.com"}'

curl https://localhost:5001/api/strategy-advanced/providers
```

## Response Formats

### Demo Response
```json
{
  "pattern": "Singleton",
  "description": "Demonstrates singleton pattern...",
  "result": { ... },
  "metadata": { ... }
}
```

### Test Response
```json
{
  "pattern": "Singleton",
  "status": "PASS",
  "checks": [
    {
      "name": "Instance ID Consistency",
      "pass": true,
      "details": "Instance IDs match: ConfigService-1-..."
    }
  ]
}
```

## Project Structure

```
DesignPatternsPlaygroundApi/
├── Controllers/          # API controllers (one per pattern)
├── Domain/              # Domain models (Order, Account, Money, etc.)
├── Infrastructure/       # Cross-cutting concerns (Metrics, Kafka, Payment Gateways)
└── Patterns/
    ├── 01-Singleton/
    │   ├── Contracts/
    │   ├── Implementations/
    │   ├── Scenarios/
    │   └── DependencyInjection.cs
    ├── 02-FactoryMethod/
    ├── ... (one folder per pattern)
    └── 15-ChainOfResponsibility/
```

## Key Features

- **Production-Grade Code**: Clean, well-documented C# with XML comments
- **FinTech Context**: Realistic trading/OMS scenarios
- **Thread-Safety Notes**: Comments explain concurrency considerations
- **Scalability Notes**: Comments explain distributed system considerations
- **Dependency Injection**: Each pattern registered via extension methods
- **Swagger/OpenAPI**: API documentation available
- **Deterministic Tests**: All tests use deterministic seeds for reproducibility

## Important Notes

### Singleton Pattern
The singleton pattern implementation includes extensive comments explaining:
- Singleton in .NET DI only ensures one instance **per application instance**
- In Kubernetes with multiple pods, each pod has its own singleton
- **This does NOT solve distributed concurrency problems**
- Correct alternatives: Database constraints, optimistic concurrency, distributed locks (with cautions)

### Testing
All `/test` endpoints execute assertions in code and return structured test results. No external test framework required for basic validation.

## Development

### Adding a New Pattern

1. Create folder structure: `Patterns/XX-PatternName/`
2. Add Contracts, Implementations, Scenarios
3. Create `DependencyInjection.cs` with extension method
4. Create controller in `Controllers/`
5. Register in `Program.cs`

### Code Quality

- All public types and methods have XML documentation
- Thread-safety and scalability considerations documented in comments
- Consistent naming conventions
- Minimal but realistic implementations

## License

This is a learning/teaching repository. Use as needed for educational purposes.

## Contributing

This is a demonstration project. Feel free to fork and extend for your own learning!
