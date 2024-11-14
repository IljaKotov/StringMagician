using Microsoft.Extensions.DependencyInjection;
using StringMagician.Handlers;
using StringMagician.Interfaces;

namespace StringMagician;

/// <summary>
/// Runs the total process of evaluating.
/// </summary>
internal class ProcessorRunner
{
	private readonly IServiceProvider _serviceProvider;
	private readonly IProcessorCore _processorCore;

	/// <summary>
	/// Initializes a new instance of the <see cref="ProcessorRunner"/> class.
	/// </summary>
	/// <param name="serviceProvider">The service provider for dependency injection.</param>
	/// <param name="processorCore">The core processor for handling input lines.</param>
	internal ProcessorRunner(IServiceProvider serviceProvider, IProcessorCore processorCore)
	{
		_serviceProvider = serviceProvider;
		_processorCore = processorCore;
	}

	/// <summary>
	/// Runs the processor by reading input, processing it, and writing output.
	/// </summary>
	public void Run()
	{
		var handler = SelectHandler();

		ArgumentNullException.ThrowIfNull(handler);

		while (true)
		{
			var inputLines = handler.ReadInput();

			if (handler.IsStopped)
				break;

			var results = inputLines.Select(line =>
				_processorCore.ProcessLine(line)).ToList();

			handler.WriteOutput(results);
		}
	}

	private IHandler? SelectHandler()
	{
		var userInterface = _serviceProvider.GetService<IUserInterface>();
		userInterface?.WriteMessage("Select mode: 1 for Interactive Mode, 2 for File Processing Mode");
		var mode = userInterface?.ReadInput();

		return mode switch
		{
			"1" => _serviceProvider.GetService<ConsoleHandler>(),
			"2" => _serviceProvider.GetService<FileHandler>(),
			_ => SelectHandler()
		};
	}
}