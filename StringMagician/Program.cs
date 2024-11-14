using Microsoft.Extensions.DependencyInjection;
using StringMagician;
using StringMagician.Core;
using StringMagician.Handlers;
using StringMagician.Interfaces;
using StringMagician.UserInterfaces;
using StringMagician.Utilities;

var serviceProvider = new ServiceCollection()
	.AddSingleton<IParser, ExpressionParser>()
	.AddSingleton<IConverter, RpnConverter>()
	.AddSingleton<IEvaluator, RpnEvaluator>()
	.AddSingleton<IEnumerable<IOperation>>(OperationFactory.CreateOperations())
	.AddSingleton<IProcessorCore, ProcessorCore>()
	.AddSingleton<IUserInterface, ConsoleUserInterface>()
	.AddSingleton<ConsoleHandler>()
	.AddSingleton<FileHandler>()
	.BuildServiceProvider();

var processorCore = serviceProvider.GetService<IProcessorCore>();

ArgumentNullException.ThrowIfNull(processorCore);

var runner = new ProcessorRunner(serviceProvider, processorCore);
runner.Run();