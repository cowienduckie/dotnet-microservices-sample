using System.Text;
using CommandService.EventProcessing;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CommandService.AsyncDataServices;

public class MessageBusSubscriber : BackgroundService
{
    private readonly IConfiguration _configuration;
    private readonly IEventProcessor _eventProcessor;
    private IConnection _connection = null!;
    private IModel _channel = null!;
    private string _queueName = null!;

    public MessageBusSubscriber(IConfiguration configuration, IEventProcessor eventProcessor)
    {
        _configuration = configuration;
        _eventProcessor = eventProcessor;

        InitializeRabbitMQ();
    }

    public override void Dispose()
    {
        if (_channel.IsOpen)
        {
            _channel.Close();
            _connection.Close();
        }

        base.Dispose();
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += (ModuleHandle, ea) =>
        {
            Console.WriteLine("--> Event Received!");

            var body = ea.Body.ToArray();
            var notificationMessage = Encoding.UTF8.GetString(body);

            Console.WriteLine($"--> Message: {notificationMessage}");

            _eventProcessor.ProcessEvent(notificationMessage);
        };

        _channel.BasicConsume(
            queue: _queueName,
            autoAck: true,
            consumer: consumer
        );

        return Task.CompletedTask;
    }

    private void InitializeRabbitMQ()
    {
        var factory = new ConnectionFactory()
        {
            HostName = _configuration["RabbitMQHost"],
            Port = int.Parse(_configuration["RabbitMQPort"])
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
        _queueName = _channel.QueueDeclare().QueueName;
        _channel.QueueBind(
            queue: _queueName,
            exchange: "trigger",
            routingKey: ""
        );

        Console.WriteLine("--> Listening on the Message Bus...");

        _connection.ConnectionShutdown += RabbitMQConnectionShutdown;
    }

    private void RabbitMQConnectionShutdown(object? sender, ShutdownEventArgs e)
    {
        Console.WriteLine("--> RabbitMQ Connection Shutdown");
    }
}