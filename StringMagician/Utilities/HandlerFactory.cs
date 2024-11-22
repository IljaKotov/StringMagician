using Microsoft.Extensions.Options;
using StringMagician.Configuration;
using StringMagician.Handlers;
using StringMagician.Interfaces;

namespace StringMagician.Utilities;

internal class HandlerFactory : IHandlerFactory
{
	private readonly IUserInterface _userInterface;
	private readonly ConsoleHandler _consoleHandler;
	private readonly FileHandler _fileHandler;
	private readonly HandlerSettings _settings;

	public HandlerFactory(IUserInterface userInterface,
		ConsoleHandler consoleHandler,
		FileHandler fileHandler,
		IOptions<HandlerSettings> settings)
	{
		_userInterface = userInterface;
		_consoleHandler = consoleHandler;
		_fileHandler = fileHandler;
		_settings = settings.Value;
	}

	public IHandler SelectHandler()
	{
		var enterTemplate = _settings.SelectModeMessage;

		_userInterface.WriteMessage(string.Format(enterTemplate, _settings.InteractiveModeCommand,
			_settings.FileProcessingModeCommand));

		var mode = _userInterface.ReadInput();

		return mode switch
		{
			var m when m == _settings.InteractiveModeCommand => _consoleHandler,
			var m when m == _settings.FileProcessingModeCommand => _fileHandler,
			_ => SelectHandler()
		};
	}
}