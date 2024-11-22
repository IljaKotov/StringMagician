using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using StringMagician.Configuration;
using StringMagician.Core;
using StringMagician.Core.Handlers;
using StringMagician.Interfaces;
using StringMagician.Operations;
using StringMagician.Utilities;

namespace StringMagician.Tests;

public class TestBase
{
	internal readonly List<IOperation> Operations;
	internal readonly ExpressionParser Parser = new();
	protected readonly ServiceProvider ServiceProvider;

	protected TestBase()
	{
		var operationSettings = Options.Create(new List<OperationSettings>
		{
			new OperationSettings
			{
				Id = "Concatenation",
				Operator = "+",
				Priority = 1
			},
			new OperationSettings
			{
				Id = "Multiplication",
				Operator = "*",
				Priority = 2
			},
			new OperationSettings
			{
				Id = "Removal",
				Operator = "-",
				Priority = 1
			}
		});

		Operations = OperationFactory.CreateOperations(operationSettings);

		var coreSettings = Options.Create(new CoreSettings
		{
			InsufficientOperands = "Insufficient operands!",
			MalformedExpression = "Malformed expression!",
			FinalStackCount = 1,
			MinimumOperands = 2,
			LeftParenthesis = "(",
			RightParenthesis = ")",
			EmptyStack = 0
		});

		ServiceProvider = new ServiceCollection()
			.AddSingleton<IEnumerable<IOperation>>(Operations)
			.AddSingleton(coreSettings)
			.AddSingleton<OperationContext>()
			.AddSingleton(provider =>
			{
				var operations = provider.GetService<IEnumerable<IOperation>>();
				ArgumentNullException.ThrowIfNull(operations);

				return operations.ToDictionary(op => op.Operator, op => op.Priority);
			})
			.AddTransient<OperandHandler>()
			.AddTransient<OperatorHandler>(provider =>
			{
				var operationPriorities = provider.GetService<Dictionary<string, int>>();
				ArgumentNullException.ThrowIfNull(operationPriorities);

				return new OperatorHandler(operationPriorities, coreSettings);
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
	}
}