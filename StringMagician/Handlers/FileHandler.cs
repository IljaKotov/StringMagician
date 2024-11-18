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
	/// Reads input from a file.
	/// </summary>
	/// <returns>A collection of input lines.</returns>
	/// <exception cref="ArgumentNullException">Thrown when the input file path is null.</exception>
	/// <exception cref="FileNotFoundException">Thrown when the input file does not exist.</exception>
	public IEnumerable<string> ReadInput()
	{
		_userInterface.WriteMessage($"Enter the path of the input file or type '{ExitCommand}' to finish:");
		var inputFilePath = _userInterface.ReadInput();

		ArgumentNullException.ThrowIfNull(inputFilePath);

		if (inputFilePath.Equals(ExitCommand, StringComparison.CurrentCultureIgnoreCase))
		{
			IsStopped = true;

			yield break;
		}

		if (File.Exists(inputFilePath) is false)
			throw new FileNotFoundException("Input file not exist.");

		foreach (var line in File.ReadLines(inputFilePath))
		{
			yield return line;
		}
	}

	/// <summary>
	/// Writes output to a file.
	/// </summary>
	/// <param name="output">The collection of output lines to be written.</param>
	/// <exception cref="ArgumentNullException">Thrown when the output file path is null.</exception>
	public void WriteOutput(IEnumerable<string> output)
	{
		_userInterface.WriteMessage("Enter output file path:");
		var outputFilePath = _userInterface.ReadInput();

		ArgumentNullException.ThrowIfNull(outputFilePath);

		File.WriteAllLines(outputFilePath, output);
		_userInterface.WriteMessage($"Output written to file {outputFilePath} successfully.");
	}
}