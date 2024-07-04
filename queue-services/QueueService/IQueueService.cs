namespace queue_services.QueueService;

public interface IQueueService<T>
{
	ValueTask AddQueue(T workItem);

	ValueTask<T> DeQueue(CancellationToken cancellationToken);
}
