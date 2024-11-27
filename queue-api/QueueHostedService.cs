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

	protected override Task ExecuteAsync(CancellationToken cancellationToken)
	{
		_logger.LogInformation("{name} is starting...", nameof(QueueHostedService));

		return StartQueueAsync(cancellationToken);
	}

	private async Task StartQueueAsync(CancellationToken cancellationToken)
	{
		while (!cancellationToken.IsCancellationRequested)
		{
			try
			{
				string name = await _queueService.RemoveFromQueueAsync(cancellationToken);

				_logger.LogInformation("Processing item: {name}", name);

				bool completed = await ProcessTaskAsync(name);

				if (completed)
					_logger.LogError("Successfully processed: {name}", name);
				else
					_logger.LogInformation("Processing failed for: {name}", name);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An error occurred while processing the queue.");
			}
		}
	}

	private async Task<bool> ProcessTaskAsync(string name)
	{
		_logger.LogInformation("Starting task for {name}", name);

		await Task.Delay(3000);

		return true;
	}

	public override async Task StopAsync(CancellationToken cancellationToken)
	{
		_logger.LogInformation("{name} is stopping", nameof(QueueHostedService));

		await base.StopAsync(cancellationToken);
	}
}