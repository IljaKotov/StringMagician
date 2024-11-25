using StringMagician.Core;
using StringMagician.Interfaces;

namespace StringMagician;

/// <summary>
/// Runs the total process of evaluating.
/// </summary>
internal class ProcessorRunner: IProcessorRunner
{
	private readonly IProcessorCore _processorCore;
	private readonly IHandlerFactory _handlerFactory;

	/// <summary>
	/// Initializes a new instance of the <see cref="ProcessorRunner"/> class.
	/// </summary>
	/// <param name="handlerFactory">The factory for selecting the appropriate handler.</param>
	/// <param name="processorCore">The core processor for handling input lines.</param>
	public ProcessorRunner(IProcessorCore processorCore, IHandlerFactory handlerFactory)
	{
		_processorCore = processorCore;
		_handlerFactory = handlerFactory;
	}

	/// <summary>
	/// Runs the processor by reading input, processing it, and writing output.
	/// </summary>
	public async Task RunAsync()
	{
		IHandler handler = _handlerFactory.SelectHandler();
		ArgumentNullException.ThrowIfNull(handler);

		await foreach (string line in handler.ReadInputAsync())
		{
			if (handler.IsStopped)
				break;

			ProcessingResult result = await _processorCore.ProcessLine(line);

			await handler.WriteOutputAsync([result]);
		}
	}
}