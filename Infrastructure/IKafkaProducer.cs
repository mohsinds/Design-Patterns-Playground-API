namespace DesignPatterns.Playground.Api.Infrastructure;

/// <summary>
/// Kafka producer interface for event publishing.
/// In production, this would use Confluent.Kafka or similar.
/// </summary>
public interface IKafkaProducer
{
    /// <summary>
    /// Publish an event to a Kafka topic.
    /// </summary>
    Task PublishAsync<T>(string topic, T message, CancellationToken cancellationToken = default) where T : class;
}

/// <summary>
/// Fake in-memory Kafka producer for demo purposes.
/// In production, this would connect to actual Kafka brokers.
/// </summary>
public class FakeKafkaProducer : IKafkaProducer
{
    private readonly ILogger<FakeKafkaProducer> _logger;
    private readonly List<(string Topic, object Message, DateTime Timestamp)> _publishedMessages = new();
    
    public FakeKafkaProducer(ILogger<FakeKafkaProducer> logger)
    {
        _logger = logger;
    }
    
    public Task PublishAsync<T>(string topic, T message, CancellationToken cancellationToken = default) where T : class
    {
        _publishedMessages.Add((topic, message, DateTime.UtcNow));
        _logger.LogInformation("Published message to topic {Topic}: {Message}", topic, typeof(T).Name);
        return Task.CompletedTask;
    }
    
    public List<(string Topic, object Message, DateTime Timestamp)> GetPublishedMessages() => _publishedMessages;
}
