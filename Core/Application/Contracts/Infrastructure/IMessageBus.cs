namespace Application.Contracts.Infrastructure;

public interface IMessageBus
{
	Task<bool> PublishAsync<T>(T message,CancellationToken cancellationToken);
}