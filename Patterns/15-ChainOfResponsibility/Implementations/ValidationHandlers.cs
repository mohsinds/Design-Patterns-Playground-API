using System.Collections.Concurrent;
using DesignPatterns.Playground.Api.Domain;
using DesignPatterns.Playground.Api.Patterns._15_ChainOfResponsibility.Contracts;

namespace DesignPatterns.Playground.Api.Patterns._15_ChainOfResponsibility.Implementations;

/// <summary>
/// Base validation handler with chain support.
/// Chain of Responsibility: base class for handlers in the chain.
/// </summary>
public abstract class BaseValidationHandler : IValidationHandler
{
    protected IValidationHandler? _next;
    
    public IValidationHandler SetNext(IValidationHandler handler)
    {
        _next = handler;
        return handler;
    }
    
    public async Task<ValidationResult> HandleAsync(Order order, CancellationToken cancellationToken = default)
    {
        // Validate current handler
        var result = await ValidateAsync(order, cancellationToken);
        
        if (!result.IsValid)
        {
            // Validation failed, return errors
            return result;
        }
        
        // Validation passed, pass to next handler
        if (_next != null)
        {
            return await _next.HandleAsync(order, cancellationToken);
        }
        
        // No more handlers, validation passed
        return result;
    }
    
    protected abstract Task<ValidationResult> ValidateAsync(Order order, CancellationToken cancellationToken);
}

/// <summary>
/// Basic validation handler (quantity, price, symbol).
/// </summary>
public class BasicValidationHandler : BaseValidationHandler
{
    protected override Task<ValidationResult> ValidateAsync(Order order, CancellationToken cancellationToken)
    {
        var errors = new List<string>();
        
        if (order.Quantity <= 0)
            errors.Add("Quantity must be greater than zero");
        
        if (order.Price <= 0)
            errors.Add("Price must be greater than zero");
        
        if (string.IsNullOrWhiteSpace(order.Symbol))
            errors.Add("Symbol is required");
        
        return Task.FromResult(new ValidationResult(errors.Count == 0, errors));
    }
}

/// <summary>
/// Risk validation handler (order size limits).
/// </summary>
public class RiskValidationHandler : BaseValidationHandler
{
    private const decimal MaxOrderValue = 1000000m;
    
    protected override Task<ValidationResult> ValidateAsync(Order order, CancellationToken cancellationToken)
    {
        var errors = new List<string>();
        var orderValue = order.Quantity * order.Price;
        
        if (orderValue > MaxOrderValue)
            errors.Add($"Order value {orderValue} exceeds maximum {MaxOrderValue}");
        
        return Task.FromResult(new ValidationResult(errors.Count == 0, errors));
    }
}

/// <summary>
/// Account validation handler (account exists, has balance).
/// </summary>
public class AccountValidationHandler : BaseValidationHandler
{
    private readonly IAccountRepository _accountRepository;
    
    public AccountValidationHandler(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }
    
    protected override async Task<ValidationResult> ValidateAsync(Order order, CancellationToken cancellationToken)
    {
        var errors = new List<string>();
        
        var account = await _accountRepository.GetByIdAsync(order.AccountId, cancellationToken);
        
        if (account == null)
            errors.Add($"Account {order.AccountId} not found");
        
        return new ValidationResult(errors.Count == 0, errors);
    }
}

/// <summary>
/// Simple account repository for demo.
/// </summary>
public interface IAccountRepository
{
    Task<Account?> GetByIdAsync(string accountId, CancellationToken cancellationToken = default);
}

/// <summary>
/// In-memory account repository.
/// </summary>
public class InMemoryAccountRepository : IAccountRepository
{
    private readonly ConcurrentDictionary<string, Account> _accounts = new();
    
    public InMemoryAccountRepository()
    {
        // Seed with test account
        _accounts["ACC-001"] = new Account("ACC-001", "Test Account", 100000m, "USD", DateTime.UtcNow);
    }
    
    public Task<Account?> GetByIdAsync(string accountId, CancellationToken cancellationToken = default)
    {
        _accounts.TryGetValue(accountId, out var account);
        return Task.FromResult(account);
    }
}
