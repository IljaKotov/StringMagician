using Microsoft.Extensions.Options;
using StringMagician.Configuration;
using StringMagician.Core;
using StringMagician.Interfaces;

namespace StringMagician.Handlers;

/// <summary>
/// Handles file input and output operations.
/// </summary>
internal class FileHandler : IHandler
{
	private readonly IUserInterface _userInterface;
	private readonly HandlerSettings _settings;

	/// <summary>
	/// Gets a value indicating whether the application is stopped.
	/// </summary>
	public bool IsStopped { get; private set; }

	/// <summary>
	/// Initializes a new instance of the <see cref="FileHandler"/> class.
	/// </summary>
	/// <param name="userInterface">The user interface to be used for input and output operations.</param>
	/// <param name="handlerSettings">The configuration to be used.</param>
	public FileHandler(IUserInterface userInterface,
		IOptions<HandlerSettings> handlerSettings)
	{
		_userInterface = userInterface;
		_settings = handlerSettings.Value;
	}

	/// <summary>
	/// Reads input from a file asynchronously.
	/// </summary>
	/// <returns>A collection of input lines.</returns>
	/// <exception cref="ArgumentNullException">Thrown when the input file path is null.</exception>
	/// <exception cref="FileNotFoundException">Thrown when the input file does not exist.</exception>
	public async IAsyncEnumerable<string> ReadInputAsync()
	{
		while (IsStopped is false)
		{
			var inputFilePath = GetInputFilePath();

			if (CheckForExitCommand(inputFilePath))
				yield break;

			await foreach (var line in ReadLinesFromFileAsync(inputFilePath))
				yield return line;
		}
	}

	/// <summary>
	/// Writes output to a file asynchronously.
	/// </summary>
	/// <param name="output">The collection of output lines to be written.</param>
	/// <exception cref="ArgumentNullException">Thrown when the output file path is null.</exception>
	public async Task WriteOutputAsync(IEnumerable<ProcessingResult> output)
	{
		var outputFilePath = GetOutputFilePath();
		var formattedOutput = output.Select(FormatProcessingResult);
		await WriteLinesToFileAsync(outputFilePath, formattedOutput);
	}

	private string GetInputFilePath()
	{
		var enterTemplate = _settings.EnterFileTemplate;
		var enterMessage = string.Format(enterTemplate, _settings.ExitCommand);

		_userInterface.WriteMessage(enterMessage);
		var inputFilePath = _userInterface.ReadInput();

		ArgumentNullException.ThrowIfNull(inputFilePath);

		return inputFilePath;
	}

	private bool CheckForExitCommand(string inputFilePath)
	{
		if (inputFilePath.Equals(_settings.ExitCommand, StringComparison.CurrentCultureIgnoreCase) is false)
			return false;

		IsStopped = true;
		_userInterface.WriteMessage(_settings.GoodBye);

		return true;
	}

	private async IAsyncEnumerable<string> ReadLinesFromFileAsync(string inputFilePath)
	{
		if (File.Exists(inputFilePath) is false)
			throw new FileNotFoundException(_settings.FileNonexistent);

		using var reader = new StreamReader(inputFilePath);

		while (await reader.ReadLineAsync() is { } line)
		{
			yield return line;
		}
	}

	private string GetOutputFilePath()
	{
		_userInterface.WriteMessage(_settings.OutputFileTemplate);
		var outputFilePath = _userInterface.ReadInput();
		ArgumentNullException.ThrowIfNull(outputFilePath);

		return outputFilePath;
	}

	private async Task WriteLinesToFileAsync(string outputFilePath, IEnumerable<string> lines)
	{
		await File.WriteAllLinesAsync(outputFilePath, lines);

		var enterTemplate = _settings.WriteFileReport;
		_userInterface.WriteMessage(string.Format(enterTemplate, outputFilePath));
	}

	private string FormatProcessingResult(ProcessingResult result)
	{
		var resultTemplate = _settings.ResultFileLineTemplate;
		var errorTemplate = _settings.ErrorFileReport;

		return string.IsNullOrWhiteSpace(result.ErrorMessage)
			? string.Format(resultTemplate, result.OriginalOperation, result.Result)
			: string.Format(errorTemplate, result.OriginalOperation, result.ErrorMessage);
	}
}