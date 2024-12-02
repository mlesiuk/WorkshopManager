namespace workshopManager.Domain.Abstractions.Interfaces;

public interface IMessageSubscriber
{
    void Subscribe<T>(string topic, Func<T, Task> handler);
}
