using DesignPatterns.Playground.Api.Patterns._06_Command.Contracts;
using DesignPatterns.Playground.Api.Patterns._06_Command.Implementations;
using DesignPatterns.Playground.Api.Patterns._06_Command.Scenarios;
using Microsoft.Extensions.DependencyInjection;

namespace DesignPatterns.Playground.Api.Patterns._06_Command;

public static class DependencyInjection
{
    public static IServiceCollection AddCommandPattern(this IServiceCollection services)
    {
        services.AddSingleton<IOrderRepository, InMemoryOrderRepository>();
        services.AddScoped<ICommandHandler, CommandHandler>();
        services.AddScoped<CommandScenario>();
        
        return services;
    }
}
