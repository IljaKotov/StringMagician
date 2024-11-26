using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using StringMagician.Configuration;
using StringMagician.Core;
using StringMagician.Core.Handlers;
using StringMagician.Handlers;
using StringMagician.Interfaces;
using StringMagician.Operations;
using StringMagician.UserInterfaces;
using StringMagician.Utilities;

namespace StringMagician.Tests;

public class TestBase
{
    internal readonly List<IOperation> Operations;
    internal readonly ExpressionParser Parser = new();
    protected readonly ServiceProvider ServiceProvider;

    protected TestBase()
    {
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

        services.AddSingleton<IOperationContext, OperationContext>();
        services.AddSingleton<IEnumerable<IOperation>>(provider =>
        {
            IOptions<List<OperationSettings>>? operationSettings = provider.GetService<IOptions<List<OperationSettings>>>();
            ArgumentNullException.ThrowIfNull(operationSettings);

            return OperationFactory.CreateOperations(operationSettings);
        });

        services.AddSingleton<IProcessorCore, ProcessorCore>();

        services.AddSingleton<IUserInterface, ConsoleUserInterface>();
        services.AddKeyedSingleton<IHandler, ConsoleHandler>(HandlerType.Console);
        services.AddKeyedSingleton<IHandler, FileHandler>(HandlerType.File);
        services.AddSingleton<IHandlerFactory, HandlerFactory>();

        services.AddSingleton<IProcessorRunner, ProcessorRunner>();

        services.AddTransient<IDictionary<string, int>>(provider =>
        {
            IEnumerable<IOperation>? operations = provider.GetService<IEnumerable<IOperation>>();
            ArgumentNullException.ThrowIfNull(operations);

            return operations.ToDictionary(op => op.Operator, op => op.Priority);
        });

        services.AddKeyedTransient<IChainHandler, OperandHandler>(ChainHandlerType.Operand);
        services.AddKeyedTransient<IChainHandler, OperatorHandler>(ChainHandlerType.Operator);
        services.AddKeyedTransient<IChainHandler, LeftParenthesisHandler>(ChainHandlerType.LeftParenthesis);
        services.AddKeyedTransient<IChainHandler, RightParenthesisHandler>(ChainHandlerType.RightParenthesis);

        services.AddSingleton<IChainHandlerFactory, ChainHandlerFactory>();
        services.AddSingleton<IChainHandler>(provider =>
        {
            IChainHandlerFactory factory = provider.GetRequiredService<IChainHandlerFactory>();
            return factory.CreateChainHandler();
        });

        services.AddTransient<IConverter, RpnConverter>();

        ServiceProvider = services.BuildServiceProvider();

        Operations = ServiceProvider.GetService<IEnumerable<IOperation>>()?.ToList() ?? new List<IOperation>();
    }
}