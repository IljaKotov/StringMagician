using Microsoft.Extensions.Options;
using StringMagician.Configuration;
using StringMagician.Core;
using StringMagician.Interfaces;

namespace StringMagician.Handlers;

/// <summary>
/// Handles console input and output operations.
/// </summary>
internal class ConsoleHandler : IHandler
{
	private readonly IUserInterface _userInterface;
	private readonly HandlerSettings _settings;

	/// <summary>
	/// Gets a value indicating whether the application is stopped.
	/// </summary>
	public bool IsStopped { get; private set; }

	/// <summary>
	/// Initializes a new instance of the <see cref="ConsoleHandler"/> class.
	/// </summary>
	/// <param name="userInterface">The user interface to be used.</param>
	/// <param name="handlerSettings">The configuration to be used.</param>
	public ConsoleHandler(IUserInterface userInterface,
		IOptions<HandlerSettings> handlerSettings)
	{
		_userInterface = userInterface;
		_settings = handlerSettings.Value;
	}

	/// <summary>
	/// Reads input from the console asynchronously.
	/// </summary>
	/// <returns>A collection of input lines.</returns>
	/// <exception cref="ArgumentNullException">Thrown when the input is null.</exception>
	public async IAsyncEnumerable<string> ReadInputAsync()
	{
		while (IsStopped is false)
		{
			string input = await GetInputAsync();

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
		foreach (ProcessingResult result in output)
		{
			string message = FormatProcessingResult(result);
			await WriteMessageAsync(message);
		}
	}

	private Task<string> GetInputAsync()
	{
		string enterTemplate = _settings.EnterConsoleTemplate;
		string enterMessage = string.Format(enterTemplate, _settings.ExitCommand);

		_userInterface.WriteMessage(enterMessage);

		string input = _userInterface.ReadInput();
		ArgumentNullException.ThrowIfNull(input);

		return Task.FromResult(input);
	}

	private bool CheckForExitCommand(string input)
	{
		if (input.Equals(_settings.ExitCommand, StringComparison.CurrentCultureIgnoreCase) is false)
			return false;

		IsStopped = true;
		_userInterface.WriteMessage(_settings.GoodBye);

		return true;
	}

	private Task WriteMessageAsync(string message)
	{
		_userInterface.WriteMessage(message);
		return Task.CompletedTask;
	}

	private string FormatProcessingResult(ProcessingResult result)
	{
		string resultTemplate = _settings.ResultConsoleMessage;
		string errorMessage = _settings.ErrorConsoleMessage;

		return string.IsNullOrWhiteSpace(result.ErrorMessage)
			? string.Format(resultTemplate, result.OriginalOperation, result.Result)
			: string.Format(errorMessage, result.OriginalOperation, result.ErrorMessage);
	}
}