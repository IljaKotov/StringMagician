using StringMagician.Interfaces;

namespace StringMagician.UserInterfaces;

/// <summary>
/// Provides methods for console input and output operations.
/// </summary>
internal class ConsoleUserInterface : IUserInterface
{
	/// <summary>
	/// Writes a message to the console.
	/// </summary>
	/// <param name="message">The message to be written.</param>
	public void WriteMessage(string message)
	{
		Console.WriteLine(message);
	}

	/// <summary>
	/// Reads input from the console.
	/// </summary>
	/// <returns>The input read from the console.</returns>
	public string ReadInput()
	{
		return Console.ReadLine() ?? string.Empty;
	}
}