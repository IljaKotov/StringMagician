using StringMagician.Handlers;
using StringMagician.Interfaces;

namespace StringMagician.Utilities;

internal class HandlerFactory
{
	private readonly IUserInterface _userInterface;
	private readonly ConsoleHandler _consoleHandler;
	private readonly FileHandler _fileHandler;

	public HandlerFactory(IUserInterface userInterface, ConsoleHandler consoleHandler, FileHandler fileHandler)
	{
		_userInterface = userInterface;
		_consoleHandler = consoleHandler;
		_fileHandler = fileHandler;
	}
	public IHandler SelectHandler()
	{
		_userInterface.WriteMessage("Select mode: 1 for Interactive Mode, 2 for File Processing Mode");
		var mode = _userInterface.ReadInput();

		return mode switch
		{
			"1" => _consoleHandler,
			"2" => _fileHandler,
			_ => SelectHandler()
		};
	}
}