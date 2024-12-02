namespace workshopManager.Domain.Abstractions.Interfaces;

public interface IMessagePublisher
{
    Task PublishAsync<T>(T message);
}
