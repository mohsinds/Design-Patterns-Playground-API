using DesignPatterns.Playground.Api.Domain;
using DesignPatterns.Playground.Api.Patterns._12_Mediator.Contracts;
using DesignPatterns.Playground.Api.Patterns._12_Mediator.Implementations;
using DesignPatterns.Playground.Api.Patterns._12_Mediator.Scenarios;
using Microsoft.Extensions.DependencyInjection;

namespace DesignPatterns.Playground.Api.Patterns._12_Mediator;

public static class DependencyInjection
{
    public static IServiceCollection AddMediatorPattern(this IServiceCollection services)
    {
        services.AddScoped<IMediator, Mediator>();
        services.AddScoped<IRequestHandler<GetOrderRequest, Order?>, GetOrderHandler>();
        services.AddScoped<IRequestHandler<CreateOrderRequest, Order>, CreateOrderHandler>();
        services.AddScoped<MediatorScenario>();
        
        return services;
    }
}
