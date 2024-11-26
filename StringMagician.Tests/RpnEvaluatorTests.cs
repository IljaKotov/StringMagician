using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using StringMagician.Configuration;
using StringMagician.Core;
using StringMagician.Interfaces;

namespace StringMagician.Tests;

public class RpnEvaluatorTests : TestBase
{
	private readonly RpnConverter? _converter;
	private readonly RpnEvaluator _evaluator;

	public RpnEvaluatorTests()
	{
		_converter = ServiceProvider.GetService<IConverter>() as RpnConverter;
		IOperationContext? context = ServiceProvider.GetService<IOperationContext>();
		ArgumentNullException.ThrowIfNull(context);

		_evaluator = new RpnEvaluator(Operations, context,
			ServiceProvider.GetService<IOptions<CoreSettings>>() ?? throw new InvalidOperationException());
	}

	[Theory]
	[MemberData(nameof(TestCaseGenerator.GetTestData), MemberType = typeof(TestCaseGenerator))]
	public void TestOperations(TestCase testCase)
	{
		string result = EvaluateAsync(testCase.Expression);
		result.Should().Be(testCase.ExpectedResult);
	}

	private string EvaluateAsync(string expression)
	{
		List<string> tokens = Parser.Parse(expression).ToList();
		List<string>? rpnTokens = _converter?.ConvertToRpn(tokens).ToList();
		ArgumentNullException.ThrowIfNull(rpnTokens);

		return _evaluator.Evaluate(rpnTokens);
	}
}