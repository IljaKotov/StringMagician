using FluentAssertions;
using StringMagician.Core;
using StringMagician.Interfaces;
using StringMagician.Utilities;

namespace StringMagician.Tests;

public class RpnConverterTests
{
	private readonly List<IOperation> _operations = OperationFactory.CreateOperations();
	private readonly ExpressionParser _parser = new();
	private readonly RpnConverter _converter;

	public RpnConverterTests()
	{
		_converter = new RpnConverter(_operations);
	}

	[Theory]
	[MemberData(nameof(TestCaseGenerator.GetTestData), MemberType = typeof(TestCaseGenerator))]
	public void TestRpnConversion(TestCase testCase)
	{
		var tokens = _parser.Parse(testCase.Expression);
		var rpnTokens = _converter.ConvertToRpn(tokens);
		rpnTokens.Should().BeEquivalentTo(testCase.ExpectedRpnTokens);
	}
}