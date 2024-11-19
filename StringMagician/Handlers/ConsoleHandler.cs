using StringMagician.Core;
using StringMagician.Interfaces;

namespace StringMagician.Handlers;

/// <summary>
/// Handles console input and output operations.
/// </summary>
internal class ConsoleHandler : IHandler
{
	private const string ExitCommand = "exit";
	private readonly IUserInterface _userInterface;

	/// <summary>
	/// Gets a value indicating whether the application is stopped.
	/// </summary>
	public bool IsStopped { get; private set; }

	/// <summary>
	/// Initializes a new instance of the <see cref="ConsoleHandler"/> class.
	/// </summary>
	/// <param name="userInterface">The user interface to be used.</param>
	public ConsoleHandler(IUserInterface userInterface)
	{
		_userInterface = userInterface;
	}

	/// <summary>
	/// Reads input from the console.
	/// </summary>
	/// <returns>A collection of input lines.</returns>
	/// <exception cref="ArgumentNullException">Thrown when the input is null.</exception>
	public IEnumerable<string> ReadInput()
	{
		_userInterface.WriteMessage($"Enter expression or type '{ExitCommand}' to finish:");
		var input = _userInterface.ReadInput();

		ArgumentNullException.ThrowIfNull(input);

		if (input.Equals(ExitCommand, StringComparison.CurrentCultureIgnoreCase))
		{
			IsStopped = true;

			yield break;
		}

		if (string.IsNullOrWhiteSpace(input) is false)
			yield return input;
	}

	/// <summary>
	/// Writes output to the console.
	/// </summary>
	/// <param name="output">The collection of output lines to be written.</param>
	public void WriteOutput(IEnumerable<ProcessingResult> output)
	{
		foreach (var result in output)
		{
			_userInterface.WriteMessage(FormatProcessingResult(result));
		}
	}
	
	private static string FormatProcessingResult(ProcessingResult result)
	{
		return string.IsNullOrWhiteSpace(result.ErrorMessage) ? 
			$"Original expression: {result.OriginalOperation}\nResult: {result.Result}" :
			$"Error processing expression: {result.OriginalOperation}\nError message: {result.ErrorMessage}";
	}
}