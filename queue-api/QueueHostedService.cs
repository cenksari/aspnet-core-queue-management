namespace queue_api;

using queue_services.QueueService;

public class QueueHostedService
	(
		ILogger<QueueHostedService> logger,
		IQueueService<string> queueService
	) : BackgroundService
{
	private readonly ILogger<QueueHostedService> _logger = logger;

	private readonly IQueueService<string> _queueService = queueService;

	protected override async Task ExecuteAsync(CancellationToken cancellationToken)
	{
		while (!cancellationToken.IsCancellationRequested)
		{
			var name = await _queueService.DeQueue(cancellationToken);

			_logger.LogInformation("Queue management service worked for {name}", name);
		}
	}
}