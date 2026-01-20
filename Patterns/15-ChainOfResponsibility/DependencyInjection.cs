using DesignPatterns.Playground.Api.Patterns._15_ChainOfResponsibility.Contracts;
using DesignPatterns.Playground.Api.Patterns._15_ChainOfResponsibility.Implementations;
using DesignPatterns.Playground.Api.Patterns._15_ChainOfResponsibility.Scenarios;
using Microsoft.Extensions.DependencyInjection;

namespace DesignPatterns.Playground.Api.Patterns._15_ChainOfResponsibility;

public static class DependencyInjection
{
    public static IServiceCollection AddChainOfResponsibilityPattern(this IServiceCollection services)
    {
        // Build validation chain
        services.AddSingleton<IValidationHandler>(sp =>
        {
            var accountRepo = sp.GetRequiredService<IAccountRepository>();
            
            var basicHandler = new BasicValidationHandler();
            var riskHandler = new RiskValidationHandler();
            var accountHandler = new AccountValidationHandler(accountRepo);
            
            // Chain: Basic -> Risk -> Account
            basicHandler.SetNext(riskHandler).SetNext(accountHandler);
            
            return basicHandler;
        });
        
        services.AddSingleton<IAccountRepository, InMemoryAccountRepository>();
        services.AddScoped<ChainOfResponsibilityScenario>();
        
        return services;
    }
}
