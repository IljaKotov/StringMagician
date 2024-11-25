using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using StringMagician;
using StringMagician.Configuration;
using StringMagician.Core;
using StringMagician.Core.Handlers;
using StringMagician.Handlers;
using StringMagician.Interfaces;
using StringMagician.Operations;
using StringMagician.UserInterfaces;
using StringMagician.Utilities;

IConfigurationRoot configuration = new ConfigurationBuilder()
	.SetBasePath(Directory.GetCurrentDirectory())
	.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
	.Build();

ServiceCollection services = new();
services.AddSingleton<IConfiguration>(configuration);

services.Configure<CoreSettings>(configuration.GetSection("Settings:Core").Bind);
services.Configure<HandlerSettings>(configuration.GetSection("Settings:Handlers").Bind);
services.Configure<List<OperationSettings>>(configuration.GetSection("Settings:Operations").Bind);

services.AddTransient<IParser, ExpressionParser>();
services.AddTransient<IEvaluator, RpnEvaluator>();

services.AddSingleton<OperationContext>();
services.AddSingleton<IEnumerable<IOperation>>(provider =>
{
	IOptions<List<OperationSettings>>? operationSettings = provider.GetService<IOptions<List<OperationSettings>>>();
	ArgumentNullException.ThrowIfNull(operationSettings);

	return OperationFactory.CreateOperations(operationSettings);
});

services.AddSingleton<IProcessorCore, ProcessorCore>();

services.AddSingleton<IUserInterface, ConsoleUserInterface>();
services.AddKeyedSingleton<IHandler,ConsoleHandler>(HandlerType.Console);
services.AddKeyedSingleton<IHandler,FileHandler>(HandlerType.File);
services.AddSingleton<IHandlerFactory, HandlerFactory>();

services.AddSingleton<ProcessorRunner>();

services.AddSingleton(provider =>
{
	IEnumerable<IOperation>? operations = provider.GetService<IEnumerable<IOperation>>();
	ArgumentNullException.ThrowIfNull(operations);

	return operations.ToDictionary(op => op.Operator, op => op.Priority);
});

services.AddTransient<OperandHandler>();
services.AddTransient<OperatorHandler>(provider =>
{
	Dictionary<string, int>? operationsPriorities = provider.GetService<Dictionary<string, int>>();
	IOptions<CoreSettings>? coreSettings = provider.GetService<IOptions<CoreSettings>>();
	ArgumentNullException.ThrowIfNull(operationsPriorities);
	ArgumentNullException.ThrowIfNull(coreSettings);

	return new OperatorHandler(operationsPriorities, coreSettings);
});
services.AddTransient<LeftParenthesisHandler>();
services.AddTransient<RightParenthesisHandler>();

services.AddSingleton<IChainHandler>(provider =>
{
	OperandHandler? operandHandler = provider.GetService<OperandHandler>();
	OperatorHandler? operatorHandler = provider.GetService<OperatorHandler>();
	LeftParenthesisHandler? leftParenthesisHandler = provider.GetService<LeftParenthesisHandler>();
	RightParenthesisHandler? rightParenthesisHandler = provider.GetService<RightParenthesisHandler>();

	ArgumentNullException.ThrowIfNull(operandHandler);
	ArgumentNullException.ThrowIfNull(operatorHandler);
	ArgumentNullException.ThrowIfNull(leftParenthesisHandler);
	ArgumentNullException.ThrowIfNull(rightParenthesisHandler);

	operandHandler.SetNext(operatorHandler)
		.SetNext(leftParenthesisHandler)
		.SetNext(rightParenthesisHandler);

	return operandHandler;
});

services.AddTransient<IConverter, RpnConverter>();

ServiceProvider serviceProvider = services.BuildServiceProvider();

ProcessorRunner? runner = serviceProvider.GetService<ProcessorRunner>();
ArgumentNullException.ThrowIfNull(runner);
await runner.RunAsync();

