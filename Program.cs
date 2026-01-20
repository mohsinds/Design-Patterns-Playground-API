using DesignPatterns.Playground.Api.Infrastructure;
using DesignPatterns.Playground.Api.Patterns._01_Singleton;
using DesignPatterns.Playground.Api.Patterns._02_FactoryMethod;
using DesignPatterns.Playground.Api.Patterns._03_AbstractFactory;
using DesignPatterns.Playground.Api.Patterns._04_Builder;
using DesignPatterns.Playground.Api.Patterns._05_Adapter;
using DesignPatterns.Playground.Api.Patterns._06_Command;
using DesignPatterns.Playground.Api.Patterns._07_Decorator;
using DesignPatterns.Playground.Api.Patterns._08_Strategy;
using DesignPatterns.Playground.Api.Patterns._09_Observer;
using DesignPatterns.Playground.Api.Patterns._10_Facade;
using DesignPatterns.Playground.Api.Patterns._11_Repository;
using DesignPatterns.Playground.Api.Patterns._12_Mediator;
using DesignPatterns.Playground.Api.Patterns._13_State;
using DesignPatterns.Playground.Api.Patterns._14_Prototype;
using DesignPatterns.Playground.Api.Patterns._15_ChainOfResponsibility;
using DesignPatterns.Playground.Api.Patterns.StrategyAdvanced;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Design Patterns Playground API",
        Version = "v1",
        Description = "ASP.NET Core Web API demonstrating 15 design patterns in a FinTech context",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Design Patterns Playground"
        }
    });
});

// Add infrastructure
builder.Services.AddSingleton<IMetrics, InMemoryMetrics>();
builder.Services.AddSingleton<IKafkaProducer, FakeKafkaProducer>();
builder.Services.AddSingleton<IPaymentGateway, FakeStripeGateway>(); // Default gateway

// Register all design patterns
builder.Services.AddSingletonPattern();
builder.Services.AddFactoryMethodPattern();
builder.Services.AddAbstractFactoryPattern();
builder.Services.AddBuilderPattern();
builder.Services.AddAdapterPattern();
builder.Services.AddCommandPattern();
builder.Services.AddDecoratorPattern();
builder.Services.AddStrategyPattern();
builder.Services.AddObserverPattern();
builder.Services.AddFacadePattern();
builder.Services.AddRepositoryPattern();
builder.Services.AddMediatorPattern();
builder.Services.AddStatePattern();
    builder.Services.AddPrototypePattern();
    builder.Services.AddChainOfResponsibilityPattern();
    
    // Strategy with Resolver/Factory Pattern - Dynamic provider selection at runtime
    builder.Services.AddStrategyAdvancedPattern();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Design Patterns Playground API v1");
        c.RoutePrefix = string.Empty; // Swagger UI at root
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
