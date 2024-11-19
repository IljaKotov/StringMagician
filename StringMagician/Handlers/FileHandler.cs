using StringMagician.Core;
using StringMagician.Interfaces;

namespace StringMagician.Handlers;

/// <summary>
/// Handles file input and output operations.
/// </summary>
internal class FileHandler : IHandler
{
	private const string ExitCommand = "exit";
	private readonly IUserInterface _userInterface;

	/// <summary>
	/// Gets a value indicating whether the application is stopped.
	/// </summary>
	public bool IsStopped { get; private set; }

	/// <summary>
	/// Initializes a new instance of the <see cref="FileHandler"/> class.
	/// </summary>
	/// <param name="userInterface">The user interface to be used for input and output operations.</param>
	public FileHandler(IUserInterface userInterface)
	{
		_userInterface = userInterface;
	}

	/// <summary>
	/// Reads input from a file asynchronously.
	/// </summary>
	/// <returns>A collection of input lines.</returns>
	/// <exception cref="ArgumentNullException">Thrown when the input file path is null.</exception>
	/// <exception cref="FileNotFoundException">Thrown when the input file does not exist.</exception>
	public async IAsyncEnumerable<string> ReadInputAsync()
	{
		while (true)
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
		var formattedOutput = FormatOutput(output);
		await WriteLinesToFileAsync(outputFilePath, formattedOutput);
	}

	private string GetInputFilePath()
	{
		_userInterface.WriteMessage($"Enter the path of the input file or type '{ExitCommand}' to finish:");
		var inputFilePath = _userInterface.ReadInput();
		ArgumentNullException.ThrowIfNull(inputFilePath);

		return inputFilePath;
	}

	private bool CheckForExitCommand(string inputFilePath)
	{
		if (inputFilePath.Equals(ExitCommand, StringComparison.CurrentCultureIgnoreCase) is false)
			return false;

		IsStopped = true;

		return true;
	}

	private static async IAsyncEnumerable<string> ReadLinesFromFileAsync(string inputFilePath)
	{
		if (File.Exists(inputFilePath) is false)
			throw new FileNotFoundException("Input file does not exist.");

		using var reader = new StreamReader(inputFilePath);

		while (await reader.ReadLineAsync() is { } line)
		{
			yield return line;
		}
	}

	private string GetOutputFilePath()
	{
		_userInterface.WriteMessage("Enter output file path:");
		var outputFilePath = _userInterface.ReadInput();
		ArgumentNullException.ThrowIfNull(outputFilePath);

		return outputFilePath;
	}

	private static IEnumerable<string> FormatOutput(IEnumerable<ProcessingResult> output)
	{
		return output.Select(FormatProcessingResult);
	}

	private async Task WriteLinesToFileAsync(string outputFilePath, IEnumerable<string> lines)
	{
		await File.WriteAllLinesAsync(outputFilePath, lines);
		_userInterface.WriteMessage($"Output written to file {outputFilePath} successfully.");
	}

	private static string FormatProcessingResult(ProcessingResult result)
	{
		return string.IsNullOrWhiteSpace(result.ErrorMessage)
			? $"Original expression: {result.OriginalOperation}\nResult: {result.Result}"
			: $"Error processing expression: {result.OriginalOperation}\nError message: {result.ErrorMessage}";
	}
}