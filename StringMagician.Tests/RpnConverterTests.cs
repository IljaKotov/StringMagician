using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using StringMagician.Core;
using StringMagician.Interfaces;

namespace StringMagician.Tests;

public class RpnConverterTests : TestBase
{
	private readonly RpnConverter? _converter;

	public RpnConverterTests()
	{
		_converter = ServiceProvider.GetService<IConverter>() as RpnConverter;
	}

	[Theory]
	[MemberData(nameof(TestCaseGenerator.GetTestData), MemberType = typeof(TestCaseGenerator))]
	public async Task TestRpnConversion(TestCase testCase)
	{
		var tokens = await Parser.ParseAsync(testCase.Expression).ToListAsync();

		var rpnTokens = new List<string>();
		ArgumentNullException.ThrowIfNull(_converter);

		await foreach (var rpnToken in _converter.ConvertToRpnAsync(tokens.ToAsyncEnumerable()).ConfigureAwait(false))
		{
			rpnTokens.Add(rpnToken);
		}

		rpnTokens.Should().BeEquivalentTo(testCase.ExpectedRpnTokens, options => options.WithStrictOrdering());
	}
}