namespace queue_services.QueueService;

public interface IQueueService<T>
{
	ValueTask AddToQueueAsync(T workItem);

	ValueTask<T> RemoveFromQueueAsync(CancellationToken cancellationToken);
}