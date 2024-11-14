using FluentAssertions;
using StringMagician.Core;
using StringMagician.Interfaces;
using StringMagician.Utilities;

namespace StringMagician.Tests;

public class RpnEvaluatorTests
{
	private readonly List<IOperation> _operations = OperationFactory.CreateOperations();
	private readonly ExpressionParser _parser = new();
	private readonly RpnConverter _converter;
	private readonly RpnEvaluator _evaluator;

	public RpnEvaluatorTests()
	{
		_converter = new RpnConverter(_operations);
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
		var rpnTokens = _converter.ConvertToRpn(tokens);

		return _evaluator.Evaluate(rpnTokens);
	}
}