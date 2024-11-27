namespace queue_api.Controllers;

using Microsoft.AspNetCore.Mvc;
using queue_services.QueueService;

[ApiController, Route("api/[controller]/[action]")]
public class QueueController(IQueueService<string> queueService) : ControllerBase
{
	public readonly IQueueService<string> _queueService = queueService;

	/// <summary>
	/// Add items to queue
	/// </summary>
	/// <param name="names">String array</param>
	[HttpPost]
	public async Task<IActionResult> Add([FromBody] string[] names)
	{
		if (names == null || names.Length == 0)
			return BadRequest("The names array cannot be null or empty.");

		foreach (var name in names)
			await _queueService.AddToQueueAsync(name);

		return Ok(new { Message = "Items added to the queue successfully." });
	}
}