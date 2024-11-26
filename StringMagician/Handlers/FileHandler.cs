using System.Runtime.CompilerServices;
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
	/// <param name="cancellationToken"></param>
	/// <returns>A collection of input lines.</returns>
	/// <exception cref="ArgumentNullException">Thrown when the input file path is null.</exception>
	/// <exception cref="FileNotFoundException">Thrown when the input file does not exist.</exception>
	public async IAsyncEnumerable<string> ReadInputAsync([EnumeratorCancellation] CancellationToken cancellationToken)
	{
		while (cancellationToken.IsCancellationRequested is false)
		{
			string inputFilePath = GetInputFilePath();

			if (CheckForExitCommand(inputFilePath))
			{
				cancellationToken.ThrowIfCancellationRequested();

				yield break;
			}

			await foreach (string line in ReadLinesFromFileAsync(inputFilePath, cancellationToken))
				yield return line;
		}
	}

	/// <summary>
	/// Writes output to a file asynchronously.
	/// </summary>
	/// <param name="output">The collection of output lines to be written.</param>
	/// <param name="cancellationToken"></param>
	/// <exception cref="ArgumentNullException">Thrown when the output file path is null.</exception>
	public async Task WriteOutputAsync(IEnumerable<ProcessingResult> output, CancellationToken cancellationToken)
	{
		string outputFilePath = GetOutputFilePath();
		IEnumerable<string> formattedOutput = output.Select(FormatProcessingResult);
		await WriteLinesToFileAsync(outputFilePath, formattedOutput, cancellationToken);
	}

	private string GetInputFilePath()
	{
		string enterTemplate = _settings.EnterFileTemplate;
		string enterMessage = string.Format(enterTemplate, _settings.ExitCommand);

		_userInterface.WriteMessage(enterMessage);
		string inputFilePath = _userInterface.ReadInput();

		ArgumentNullException.ThrowIfNull(inputFilePath);

		return inputFilePath;
	}

	private bool CheckForExitCommand(string inputFilePath)
	{
		if (inputFilePath.Equals(_settings.ExitCommand, StringComparison.CurrentCultureIgnoreCase) is false)
			return false;

		_userInterface.WriteMessage(_settings.GoodBye);

		return true;
	}

	private async IAsyncEnumerable<string> ReadLinesFromFileAsync(string inputFilePath,
		[EnumeratorCancellation] CancellationToken cancellationToken)
	{
		if (File.Exists(inputFilePath) is false)
			throw new FileNotFoundException(_settings.FileNonexistent);

		using StreamReader reader = new(inputFilePath);

		while (await reader.ReadLineAsync(cancellationToken) is { } line)
		{
			if (cancellationToken.IsCancellationRequested)
				yield break;

			yield return line;
		}
	}

	private string GetOutputFilePath()
	{
		_userInterface.WriteMessage(_settings.OutputFileTemplate);
		string outputFilePath = _userInterface.ReadInput();
		ArgumentNullException.ThrowIfNull(outputFilePath);

		return outputFilePath;
	}

	private async Task WriteLinesToFileAsync(string outputFilePath,
		IEnumerable<string> lines,
		CancellationToken cancellationToken)
	{
		await File.WriteAllLinesAsync(outputFilePath, lines, cancellationToken);

		string enterTemplate = _settings.WriteFileReport;
		_userInterface.WriteMessage(string.Format(enterTemplate, outputFilePath));
	}

	private string FormatProcessingResult(ProcessingResult result)
	{
		string resultTemplate = _settings.ResultFileLineTemplate;
		string errorTemplate = _settings.ErrorFileReport;

		return string.IsNullOrWhiteSpace(result.ErrorMessage)
			? string.Format(resultTemplate, result.OriginalOperation, result.Result)
			: string.Format(errorTemplate, result.OriginalOperation, result.ErrorMessage);
	}
}