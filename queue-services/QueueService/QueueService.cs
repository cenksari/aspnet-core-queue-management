namespace queue_services.QueueService;

using System.Threading.Channels;

/// <summary>
/// Queue management service
/// </summary>
public class QueueService : IQueueService<string>
{
	private readonly Channel<string> _queue;

	public QueueService()
	{
		const int capacity = 10;

		_queue = Channel.CreateBounded<string>(new BoundedChannelOptions(capacity)
		{
			FullMode = BoundedChannelFullMode.Wait
		});
	}

	/// <summary>
	/// Add string to queue
	/// </summary>
	/// <param name="workItem">Item</param>
	public async ValueTask AddToQueueAsync(string workItem)
	{
		ArgumentNullException.ThrowIfNull(workItem);

		await _queue.Writer.WriteAsync(workItem);
	}

	/// <summary>
	/// Remove item from queue
	/// </summary>
	/// <param name="cancellationToken">Cancellation token</param>
	public async ValueTask<string> RemoveFromQueueAsync(CancellationToken cancellationToken) => await _queue.Reader.ReadAsync(cancellationToken);
}