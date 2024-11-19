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
	/// Reads input from the console asynchronously.
	/// </summary>
	/// <returns>A collection of input lines.</returns>
	/// <exception cref="ArgumentNullException">Thrown when the input is null.</exception>
	public async IAsyncEnumerable<string> ReadInputAsync()
	{
		while (true)
		{
			var input = await GetInputAsync();

			if (CheckForExitCommand(input))
				yield break;

			if (string.IsNullOrWhiteSpace(input) is false)
				yield return input;
		}
	}

	/// <summary>
	/// Writes output to the console asynchronously.
	/// </summary>
	/// <param name="output">The collection of output lines to be written.</param>
	public async Task WriteOutputAsync(IEnumerable<ProcessingResult> output)
	{
		foreach (var result in output)
		{
			await WriteMessageAsync(FormatProcessingResult(result));
		}
	}

	private async Task<string> GetInputAsync()
	{
		_userInterface.WriteMessage($"Enter expression or type '{ExitCommand}' to finish:");
		var input = await Task.Run(() => _userInterface.ReadInput());
		ArgumentNullException.ThrowIfNull(input);

		return input;
	}

	private bool CheckForExitCommand(string input)
	{
		if (input.Equals(ExitCommand, StringComparison.CurrentCultureIgnoreCase) is false)
			return false;

		IsStopped = true;

		return true;
	}

	private async Task WriteMessageAsync(string message)
	{
		await Task.Run(() => _userInterface.WriteMessage(message));
	}

	private static string FormatProcessingResult(ProcessingResult result)
	{
		return string.IsNullOrWhiteSpace(result.ErrorMessage)
			? $"Original expression: {result.OriginalOperation}\nResult: {result.Result}"
			: $"Error processing expression: {result.OriginalOperation}\nError message: {result.ErrorMessage}";
	}
}