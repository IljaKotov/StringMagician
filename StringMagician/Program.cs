using Microsoft.Extensions.DependencyInjection;
using StringMagician;
using StringMagician.Core;
using StringMagician.Handlers;
using StringMagician.Interfaces;
using StringMagician.UserInterfaces;
using StringMagician.Utilities;

var serviceProvider = new ServiceCollection()
	.AddTransient<IParser, ExpressionParser>()
	.AddTransient<IConverter, RpnConverter>()
	.AddTransient<IEvaluator, RpnEvaluator>()
	.AddSingleton<IEnumerable<IOperation>>(OperationFactory.CreateOperations())
	.AddSingleton<IProcessorCore, ProcessorCore>()
	.AddSingleton<IUserInterface, ConsoleUserInterface>()
	.AddSingleton<ConsoleHandler>()
	.AddSingleton<FileHandler>()
	.AddSingleton<HandlerFactory>()
	.AddSingleton<ProcessorRunner>()
	.BuildServiceProvider();

var runner = serviceProvider.GetService<ProcessorRunner>();
ArgumentNullException.ThrowIfNull(runner);
runner.Run();