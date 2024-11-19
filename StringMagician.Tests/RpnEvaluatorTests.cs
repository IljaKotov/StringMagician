using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using StringMagician.Core;
using StringMagician.Core.Handlers;
using StringMagician.Interfaces;
using StringMagician.Utilities;

namespace StringMagician.Tests;

public class RpnEvaluatorTests
{
	private readonly List<IOperation> _operations = OperationFactory.CreateOperations();
	private readonly ExpressionParser _parser = new();
	private readonly RpnConverter? _converter;
	private readonly RpnEvaluator _evaluator;

	public RpnEvaluatorTests()
	{
		var serviceProvider = new ServiceCollection()
            .AddSingleton<IEnumerable<IOperation>>(_operations)
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
                return new OperatorHandler(operationPriorities);
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

        _converter = serviceProvider.GetService<IConverter>() as RpnConverter;
        _evaluator = new RpnEvaluator(_operations);
	}

	[Theory]
	[MemberData(nameof(TestCaseGenerator.GetTestData), MemberType = typeof(TestCaseGenerator))]
	public void TestOperations(TestCase testCase)
	{
		var result = Evaluate(testCase.Expression);
		result.Should().Be(testCase.ExpectedResult);
	}

	private string Evaluate(string expression)
	{
		var tokens = _parser.Parse(expression);
		var rpnTokens = _converter?.ConvertToRpn(tokens);
		ArgumentNullException.ThrowIfNull(rpnTokens);
		return _evaluator.Evaluate(rpnTokens);
	}
}