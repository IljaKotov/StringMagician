using Microsoft.Extensions.DependencyInjection;
using StringMagician;
using StringMagician.Core;
using StringMagician.Core.Handlers;
using StringMagician.Handlers;
using StringMagician.Interfaces;
using StringMagician.UserInterfaces;
using StringMagician.Utilities;

var serviceProvider = new ServiceCollection()
	.AddTransient<IParser, ExpressionParser>()
	.AddTransient<IEvaluator, RpnEvaluator>()
	.AddSingleton<IEnumerable<IOperation>>(OperationFactory.CreateOperations())
	.AddSingleton<IProcessorCore, ProcessorCore>()
	.AddSingleton<IUserInterface, ConsoleUserInterface>()
	.AddSingleton<ConsoleHandler>()
	.AddSingleton<FileHandler>()
	.AddSingleton<HandlerFactory>()
	.AddSingleton<ProcessorRunner>()
	.AddSingleton(provider =>
	{
		var operations = provider.GetService<IEnumerable<IOperation>>();
		ArgumentNullException.ThrowIfNull(operations);
		return operations.ToDictionary(op => op.Operator, op => op.Priority);
	})
	.AddTransient<OperandHandler>()
	.AddTransient<OperatorHandler>(provider =>
	{
		var operationsPriorities = provider.GetService<Dictionary<string, int>>();
		ArgumentNullException.ThrowIfNull(operationsPriorities);

		return new OperatorHandler(operationsPriorities);
	})
	.AddTransient<LeftParenthesisHandler>()
	.AddTransient<RightParenthesisHandler>()
	.AddSingleton<IChainHandler>(provider =>
	{
		var operandHandler = provider.GetService<OperandHandler>();
		var operatorHandler = provider.GetService<OperatorHandler>();
		var leftParenthesisHandler = provider.GetService<LeftParenthesisHandler>();
		var rightParenthesisHandler = provider.GetService<RightParenthesisHandler>();

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

var runner = serviceProvider.GetService<ProcessorRunner>();
ArgumentNullException.ThrowIfNull(runner);
runner.Run();