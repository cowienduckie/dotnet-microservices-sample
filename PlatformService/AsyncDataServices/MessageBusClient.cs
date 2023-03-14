using System.Text;
using System.Text.Json;
using PlatformService.Dtos;
using RabbitMQ.Client;

namespace PlatformService.AsyncDataServices;

public class MessageBusClient : IMessageBusClient
{
    private readonly IModel _channel = null!;
    private readonly IConnection _connection = null!;

    public MessageBusClient(IConfiguration configuration)
    {
        var factory = new ConnectionFactory
        {
            HostName = configuration["RabbitMQHost"],
            Port = int.Parse(configuration["RabbitMQPort"])
        };

        try
        {
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare("trigger", ExchangeType.Fanout);

            _connection.ConnectionShutdown += RabbitMqConnectionShutdown;

            Console.WriteLine("--> Connected to Message Bus");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"--> Could not connect to the Message Bus: {ex.Message}");
        }
    }

    public void PublishNewPlatform(PlatformPublishedDto platformPublishedDto)
    {
        var message = JsonSerializer.Serialize(platformPublishedDto);

        if (_connection.IsOpen)
        {
            Console.WriteLine("--> RabbitMQ Connection Open, sending message...");

            SendMessage(message);
        }
        else
        {
            Console.WriteLine("--> RabbitMQ Connection is closed, not sending...");
        }
    }

    public void Dispose()
    {
        Console.WriteLine("--> Message Bus Disposed");

        if (_channel.IsOpen)
        {
            _channel.Close();
            _connection.Close();
        }
    }

    private void SendMessage(string message)
    {
        var body = Encoding.UTF8.GetBytes(message);

        _channel.BasicPublish("trigger",
            "",
            null,
            body);

        Console.WriteLine($"--> We have sent {message}");
    }

    private void RabbitMqConnectionShutdown(object? sender, ShutdownEventArgs e)
    {
        Console.WriteLine("--> RabbitMQ Connection Shutdown");
    }
}