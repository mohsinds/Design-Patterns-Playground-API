using DesignPatterns.Playground.Api.Patterns.StrategyAdvanced.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace DesignPatterns.Playground.Api.Controllers;

/// <summary>
/// Controller for advanced strategy pattern demonstration with dynamic provider selection.
/// </summary>
[ApiController]
[Route("api/strategy-advanced")]
public sealed class StrategyAdvancedController : ControllerBase
{
    private readonly IPaymentService _paymentService;
    private readonly ILogger<StrategyAdvancedController> _logger;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="StrategyAdvancedController"/> class.
    /// </summary>
    /// <param name="paymentService">The payment service for processing payments.</param>
    /// <param name="logger">The logger instance for logging operations.</param>
    public StrategyAdvancedController(
        IPaymentService paymentService,
        ILogger<StrategyAdvancedController> logger)
    {
        _paymentService = paymentService;
        _logger = logger;
    }
    
    /// <summary>
    /// Processes a payment using the specified provider.
    /// </summary>
    /// <param name="request">The payment request containing amount, currency, provider key, and customer email.</param>
    /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
    /// <returns>
    /// 200 OK with payment result on success,
    /// 400 Bad Request if validation fails,
    /// 404 Not Found if provider is not found
    /// </returns>
    [HttpPost("process-payment")]
    [ProducesResponseType(typeof(PaymentResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ProcessPayment(
        [FromBody] PaymentRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _paymentService.ProcessPaymentAsync(request, cancellationToken);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Payment processing failed: {Message}", ex.Message);
            
            // Check if it's a provider not found error
            if (ex.Message.Contains("not found"))
            {
                var availableProviders = string.Join(", ", 
                    _paymentService.GetAvailableProviders().Select(p => p.Key));
                
                return NotFound(new ErrorResponse
                {
                    Error = $"Payment provider '{request.ProviderKey}' not found. Available providers: {availableProviders}",
                    ProviderKey = request.ProviderKey,
                    Timestamp = DateTime.UtcNow
                });
            }
            
            // Validation failure
            return BadRequest(new ErrorResponse
            {
                Error = ex.Message,
                ProviderKey = request.ProviderKey,
                Timestamp = DateTime.UtcNow
            });
        }
    }
    
    /// <summary>
    /// Gets information about all available payment providers.
    /// </summary>
    /// <returns>200 OK with a list of available providers and their capabilities.</returns>
    [HttpGet("providers")]
    [ProducesResponseType(typeof(IEnumerable<ProviderInfo>), StatusCodes.Status200OK)]
    public IActionResult GetProviders()
    {
        var providers = _paymentService.GetAvailableProviders();
        return Ok(providers);
    }
}

/// <summary>
/// Represents an error response.
/// </summary>
public record ErrorResponse
{
    /// <summary>
    /// Gets or sets the error message.
    /// </summary>
    public string Error { get; init; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the provider key that caused the error, if applicable.
    /// </summary>
    public string? ProviderKey { get; init; }
    
    /// <summary>
    /// Gets or sets the timestamp when the error occurred.
    /// </summary>
    public DateTime Timestamp { get; init; } = DateTime.UtcNow;
}
