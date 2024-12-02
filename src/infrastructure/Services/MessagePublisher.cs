using MassTransit;
using workshopManager.Domain.Abstractions.Interfaces;

namespace workshopManager.Infrastructure.Services;

public sealed class MessagePublisher(IPublishEndpoint publishEndpoint) : IMessagePublisher
{
    public async Task PublishAsync<T>(T message)
    {
        if (message is null)
        {
            return;
        }

        await publishEndpoint.Publish(message);
    }
}
