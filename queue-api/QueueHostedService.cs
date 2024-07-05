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
		_logger.LogInformation("{name} is running...", nameof(QueueHostedService));

		return StartQueueAsync(cancellationToken);
	}

	private async Task StartQueueAsync(CancellationToken cancellationToken)
	{
		while (!cancellationToken.IsCancellationRequested)
		{
			try
			{
				string name = await _queueService.RemoveFromQueueAsync(cancellationToken);

				_logger.LogInformation("Queue management service worked for {name}", name);

				bool completed = await ProcessTaskAsync(name);

				if (!completed)
					_logger.LogError("Process not completed for {name}", name);
				else
					_logger.LogInformation("Process successfully completed for {name}", name);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error occurred executing task.");
			}
		}
	}

	private async Task<bool> ProcessTaskAsync(string name)
	{
		_logger.LogInformation("Process started for {name}", name);

		Task.Delay(3000).Wait();

		return await Task.FromResult(true);
	}

	public override async Task StopAsync(CancellationToken cancellationToken)
	{
		_logger.LogInformation("{name} is stopping", nameof(QueueHostedService));

		await base.StopAsync(cancellationToken);
	}
}