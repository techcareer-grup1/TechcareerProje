

using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using TechCareer.Service.EventsRabbitMQ;

namespace TechCareer.Service.RabbitMQ;

public class RabbitMQPublisher
{
    private readonly RabbitMQClientService _rabbitMQClientService;

    public RabbitMQPublisher(RabbitMQClientService rabbitMQClientService)
    {
        _rabbitMQClientService = rabbitMQClientService;
    }

    public void Publish(VideoEducationImageCreatedEvent videoEducationImageCreatedEvent)
    {
        var channel = _rabbitMQClientService.Connect();

        var bodyString = JsonSerializer.Serialize(videoEducationImageCreatedEvent);

        var bodyByte = Encoding.UTF8.GetBytes(bodyString);

        var properties = channel.CreateBasicProperties();
        properties.Persistent = true;

        channel.BasicPublish(exchange: RabbitMQClientService.ExchangeName, routingKey: RabbitMQClientService.RoutingWatermark, basicProperties: properties, body: bodyByte);

    }
}
