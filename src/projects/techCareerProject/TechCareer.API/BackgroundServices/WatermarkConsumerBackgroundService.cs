using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Drawing;
using System.Text;
using System.Text.Json;
using TechCareer.DataAccess.Repositories.Abstracts;
using TechCareer.Service.EventsRabbitMQ;
using TechCareer.Service.RabbitMQ;

namespace TechCareer.API.BackgroundServices;

public class WatermarkConsumerBackgroundService : BackgroundService
{
    private readonly RabbitMQClientService _rabbitMQClientService;
    private readonly IVideoEduRepository _videoEduRepository;
    private readonly ILogger<WatermarkConsumerBackgroundService> _logger;

    public WatermarkConsumerBackgroundService(RabbitMQClientService rabbitMQClientService, IVideoEduRepository videoEduRepository, ILogger<WatermarkConsumerBackgroundService> logger)
    {
        _rabbitMQClientService = rabbitMQClientService;
        _videoEduRepository = videoEduRepository;
        _logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var channel = _rabbitMQClientService.Connect();

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += async (sender, args) =>
        {
            if (stoppingToken.IsCancellationRequested)
                return;

            var body = args.Body.ToArray();
            var jsonString = Encoding.UTF8.GetString(body);

            var eventMessage = JsonSerializer.Deserialize<VideoEducationImageCreatedEvent>(jsonString);

            string watermarkedImageUrl = await AddWatermarkToImage(eventMessage.ImageUrl);

           
            await UpdateVideoEducationImageUrl(eventMessage.ImageUrl, watermarkedImageUrl);
        };

        channel.BasicConsume(queue: RabbitMQClientService.QueueName, autoAck: true, consumer: consumer);

        _logger.LogInformation("RabbitMQ watermark kuyruğu dinleniyor...");

        return Task.CompletedTask;
    }

    private async Task<string> AddWatermarkToImage(string imageUrl)
    {
        using (var webClient = new HttpClient())
        {
            var imageBytes = await webClient.GetByteArrayAsync(imageUrl);
            using (var stream = new MemoryStream(imageBytes))
            {
                var image = Image.FromStream(stream);
                using (Graphics graphics = Graphics.FromImage(image))
                {
                    var watermarkText = "techcareer.net";
                    var font = new Font("Arial", 50, FontStyle.Bold);
                    var brush = new SolidBrush(Color.FromArgb(100, Color.Green));
                    var point = new PointF(image.Width - 200, image.Height - 50);
                    graphics.DrawString(watermarkText, font, brush, point);
                }

                
                string filePath = Path.Combine("", Guid.NewGuid().ToString() + ".png");
                image.Save(filePath);

                
                string newImageUrl = "" + Path.GetFileName(filePath);
                return newImageUrl;
            }
        }
    }

    private async Task UpdateVideoEducationImageUrl(string oldImageUrl, string newImageUrl)
    {
        
        var videoEducation = await _videoEduRepository.GetAsync(x => x.ImageUrl == oldImageUrl);
        if (videoEducation != null)
        {
            videoEducation.ImageUrl = newImageUrl; 
            await _videoEduRepository.UpdateAsync(videoEducation); 
        }
    }
}
