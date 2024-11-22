using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using StringMagician.Configuration;
using StringMagician.Core;
using StringMagician.Interfaces;
using StringMagician.Operations;

namespace StringMagician.Tests;

public class RpnEvaluatorTests : TestBase
{
	private readonly RpnConverter? _converter;
	private readonly RpnEvaluator _evaluator;

	public RpnEvaluatorTests()
	{
		_converter = ServiceProvider.GetService<IConverter>() as RpnConverter;
		var context = ServiceProvider.GetService<OperationContext>();
		ArgumentNullException.ThrowIfNull(context);

		_evaluator = new RpnEvaluator(Operations, context,
			ServiceProvider.GetService<IOptions<CoreSettings>>() ?? throw new InvalidOperationException());
	}

	[Theory]
	[MemberData(nameof(TestCaseGenerator.GetTestData), MemberType = typeof(TestCaseGenerator))]
	public async Task TestOperations(TestCase testCase)
	{
		var result = await EvaluateAsync(testCase.Expression);
		result.Should().Be(testCase.ExpectedResult);
	}

	private async Task<string> EvaluateAsync(string expression)
	{
		var tokens = await Parser.ParseAsync(expression).ToListAsync();
		var rpnTokens = new List<string>();
		ArgumentNullException.ThrowIfNull(_converter);

		await foreach (var rpnToken in _converter.ConvertToRpnAsync(tokens.ToAsyncEnumerable()).ConfigureAwait(false))
		{
			rpnTokens.Add(rpnToken);
		}

		return await _evaluator.EvaluateAsync(rpnTokens.ToAsyncEnumerable());
	}
}