using MassTransit;

namespace CommandService.AsyncDataService;

public class MessageBusConsumerDefinition : ConsumerDefinition<MessageBusConsumer>
{
    protected override void ConfigureConsumer(
        IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<MessageBusConsumer> consumerConfigurator)
    {
        endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
    }
}