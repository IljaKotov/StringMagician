using StringMagician.Interfaces;
using StringMagician.Utilities;

namespace StringMagician;

/// <summary>
/// Runs the total process of evaluating.
/// </summary>
internal class ProcessorRunner
{
	private readonly IProcessorCore _processorCore;
	private readonly HandlerFactory _handlerFactory;

	/// <summary>
	/// Initializes a new instance of the <see cref="ProcessorRunner"/> class.
	/// </summary>
	/// <param name="handlerFactory">The factory for selecting the appropriate handler.</param>
	/// <param name="processorCore">The core processor for handling input lines.</param>
	public ProcessorRunner(IProcessorCore processorCore, HandlerFactory handlerFactory)
	{
		_processorCore = processorCore;
		_handlerFactory = handlerFactory;
	}

	/// <summary>
	/// Runs the processor by reading input, processing it, and writing output.
	/// </summary>
	public async Task RunAsync()
	{
		var handler = _handlerFactory.SelectHandler();
		ArgumentNullException.ThrowIfNull(handler);

		await foreach (var line in handler.ReadInputAsync())
		{
			if (handler.IsStopped)
				break;

			var result = await _processorCore.ProcessLine(line);

			await handler.WriteOutputAsync(new[]
			{
				result
			});
		}
	}
}