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
	public void TestRpnConversion(TestCase testCase)
	{
		List<string> tokens = Parser.Parse(testCase.Expression).ToList();

		List<string>? rpnTokens = _converter?.ConvertToRpn(tokens).ToList();
		ArgumentNullException.ThrowIfNull(_converter);

		rpnTokens.Should().BeEquivalentTo(testCase.ExpectedRpnTokens, options => options.WithStrictOrdering());
	}
}