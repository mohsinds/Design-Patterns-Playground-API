using DesignPatterns.Playground.Api.Domain;
using DesignPatterns.Playground.Api.Patterns._11_Repository.Contracts;
using DesignPatterns.Playground.Api.Patterns._11_Repository.Implementations;
using DesignPatterns.Playground.Api.Patterns._11_Repository.Scenarios;
using Microsoft.Extensions.DependencyInjection;

namespace DesignPatterns.Playground.Api.Patterns._11_Repository;

public static class DependencyInjection
{
    public static IServiceCollection AddRepositoryPattern(this IServiceCollection services)
    {
        services.AddSingleton<IRepository<Order, string>>(sp =>
        {
            var logger = sp.GetRequiredService<ILogger<InMemoryRepository<Order, string>>>();
            return new InMemoryRepository<Order, string>(o => o.OrderId, logger);
        });
        
        services.AddScoped<IUnitOfWork, InMemoryUnitOfWork>();
        services.AddScoped<RepositoryScenario>();
        
        return services;
    }
}
