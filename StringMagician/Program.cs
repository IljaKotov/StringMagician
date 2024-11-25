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

ServiceProvider serviceProvider = new ServiceCollection()
	.AddSingleton<IConfiguration>(configuration)
	.Configure<CoreSettings>(configuration.GetSection("Settings:Core").Bind)
	.Configure<HandlerSettings>(configuration.GetSection("Settings:Handlers").Bind)
	.Configure<List<OperationSettings>>(configuration.GetSection("Settings:Operations").Bind)
	.AddTransient<IParser, ExpressionParser>()
	.AddTransient<IEvaluator, RpnEvaluator>()
	.AddSingleton<OperationContext>()
	.AddSingleton<IEnumerable<IOperation>>(provider =>
	{
		IOptions<List<OperationSettings>>? operationSettings = provider.GetService<IOptions<List<OperationSettings>>>();
		ArgumentNullException.ThrowIfNull(operationSettings);

		return OperationFactory.CreateOperations(operationSettings);
	})
	.AddSingleton<IProcessorCore, ProcessorCore>()
	.AddSingleton<IUserInterface, ConsoleUserInterface>()
	.AddSingleton<ConsoleHandler>()
	.AddSingleton<FileHandler>()
	.AddSingleton<IHandlerFactory, HandlerFactory>()
	.AddSingleton<ProcessorRunner>()
	.AddSingleton(provider =>
	{
		IEnumerable<IOperation>? operations = provider.GetService<IEnumerable<IOperation>>();
		ArgumentNullException.ThrowIfNull(operations);

		return operations.ToDictionary(op => op.Operator, op => op.Priority);
	})
	.AddTransient<OperandHandler>()
	.AddTransient<OperatorHandler>(provider =>
	{
		Dictionary<string, int>? operationsPriorities = provider.GetService<Dictionary<string, int>>();
		IOptions<CoreSettings>? coreSettings = provider.GetService<IOptions<CoreSettings>>();
		ArgumentNullException.ThrowIfNull(operationsPriorities);
		ArgumentNullException.ThrowIfNull(coreSettings);

		return new OperatorHandler(operationsPriorities, coreSettings);
	})
	.AddTransient<LeftParenthesisHandler>()
	.AddTransient<RightParenthesisHandler>()
	.AddSingleton<IChainHandler>(provider =>
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
	})
	.AddTransient<IConverter, RpnConverter>()
	.BuildServiceProvider();

ProcessorRunner? runner = serviceProvider.GetService<ProcessorRunner>();
ArgumentNullException.ThrowIfNull(runner);
await runner.RunAsync();