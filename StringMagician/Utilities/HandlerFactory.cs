using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using StringMagician.Configuration;
using StringMagician.Handlers;
using StringMagician.Interfaces;

namespace StringMagician.Utilities;

internal class HandlerFactory : IHandlerFactory
{
	private readonly IUserInterface _userInterface;
	private readonly IHandler _consoleHandler;
	private readonly IHandler _fileHandler;
	private readonly HandlerSettings _settings;

	public HandlerFactory(IUserInterface userInterface,
		[FromKeyedServices(HandlerType.Console)] IHandler consoleHandler,
		[FromKeyedServices(HandlerType.File)] IHandler fileHandler,
		IOptions<HandlerSettings> settings)
	{
		_userInterface = userInterface;
		_consoleHandler = consoleHandler;
		_fileHandler = fileHandler;
		_settings = settings.Value;
	}

	public IHandler SelectHandler()
	{
		string enterTemplate = _settings.SelectModeMessage;

		_userInterface.WriteMessage(string.Format(enterTemplate, _settings.InteractiveModeCommand,
			_settings.FileProcessingModeCommand));

		string mode = _userInterface.ReadInput();

		return mode switch
		{
			_ when mode == _settings.InteractiveModeCommand => _consoleHandler,
			_ when mode == _settings.FileProcessingModeCommand => _fileHandler,
			_ => SelectHandler()
		};
	}
}