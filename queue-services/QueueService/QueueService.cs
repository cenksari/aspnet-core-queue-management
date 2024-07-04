namespace queue_services.QueueService;

using System.Threading.Channels;

/// <summary>
/// Queue management service
/// </summary>
public class QueueService : IQueueService<string>
{
	private readonly Channel<string> queue;

	public QueueService()
	{
		int capacity = 10;

		BoundedChannelOptions options = new(capacity)
		{
			FullMode = BoundedChannelFullMode.Wait
		};

		queue = Channel.CreateBounded<string>(options);
	}

	/// <summary>
	/// Add string to queue
	/// </summary>
	/// <param name="workItem">Item</param>
	public async ValueTask AddQueue(string workItem)
	{
		ArgumentNullException.ThrowIfNull(workItem, nameof(workItem));

		await queue.Writer.WriteAsync(workItem);
	}

	/// <summary>
	/// Remove item from queue
	/// </summary>
	/// <param name="cancellationToken">Cancellation token</param>
	public async ValueTask<string> DeQueue(CancellationToken cancellationToken)
	{
		var workItem = await queue.Reader.ReadAsync(cancellationToken);

		return workItem;
	}
}