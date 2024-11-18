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
	public void Run()
	{
		var handler = _handlerFactory.SelectHandler();

		ArgumentNullException.ThrowIfNull(handler);

		while (true)
		{
			var inputLines = handler.ReadInput();

			if (handler.IsStopped) break;

			var results = ProcessInputLines(inputLines);
			handler.WriteOutput(results);
		}
	}
	

	private IEnumerable<string> ProcessInputLines(IEnumerable<string> inputLines)
	{
		return inputLines.Select(line => _processorCore.ProcessLine(line));
	}
}