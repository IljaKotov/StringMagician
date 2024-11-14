using StringMagician.Interfaces;

namespace StringMagician.Handlers;

/// <summary>
/// Handles console input and output operations.
/// </summary>
internal class ConsoleHandler : IHandler
{
	private readonly IUserInterface _userInterface;

	/// <summary>
	/// Gets a value indicating whether the application is stopped.
	/// </summary>
	public bool IsStopped { get; private set; }

	/// <summary>
	/// Initializes a new instance of the <see cref="ConsoleHandler"/> class.
	/// </summary>
	/// <param name="userInterface">The user interface to be used.</param>
	internal ConsoleHandler(IUserInterface userInterface)
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
		var inputLines = new List<string>();

		_userInterface.WriteMessage("Enter expression or type 'exit' to finish:");
		var input = _userInterface.ReadInput();

		ArgumentNullException.ThrowIfNull(input);

		if (input.Equals("exit", StringComparison.CurrentCultureIgnoreCase))
		{
			IsStopped = true;

			return Array.Empty<string>();
		}

		if (string.IsNullOrWhiteSpace(input) is false)
			inputLines.Add(input);

		return inputLines;
	}

	/// <summary>
	/// Writes output to the console.
	/// </summary>
	/// <param name="output">The collection of output lines to be written.</param>
	public void WriteOutput(IEnumerable<string> output)
	{
		foreach (var line in output)
		{
			_userInterface.WriteMessage(line);
		}
	}
}