
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace TechCareer.Service.RabbitMQ;

public class RabbitMQClientService : IDisposable
{
    private readonly ConnectionFactory _connectionFactory;
    private IConnection _connection;
    private IModel _channel;
    public static string ExchangeName = "ImageNewDirectExchange";
    public static string RoutingWatermark = "watermarknew-route-image";
    public static string QueueName = "queue-watermarknew-image";

    private readonly ILogger<RabbitMQClientService> _logger;

    public RabbitMQClientService(ConnectionFactory connectionFactory, ILogger<RabbitMQClientService> logger)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    public IModel Connect()
    {
        if (_channel is { IsOpen: true })
        {
            return _channel;
        }

        _connection = _connectionFactory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.ExchangeDeclare(ExchangeName, type: "direct", true, false);
        _channel.QueueDeclare(QueueName, true, false, false, null);
        _channel.QueueBind(exchange: ExchangeName, queue: QueueName, routingKey: RoutingWatermark);

        _logger.LogInformation("RabbitMQ ile bağlantı kuruldu...");

        return _channel;
    }

    public void Dispose()
    {
        _channel?.Close();
        _channel?.Dispose();
        _connection?.Close();
        _connection?.Dispose();

        _logger.LogInformation("RabbitMQ ile bağlantı koptu...");
    }
}
