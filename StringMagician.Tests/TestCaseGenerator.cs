namespace StringMagician.Tests;

internal static class TestCaseGenerator
{
	public static IEnumerable<object[]> GetTestData()
	{
		yield return [GenerateOnlyOperandTestCase()];

		yield return [GenerateConcatenationTestCase()];

		yield return [GenerateTotalRemovalTestCase()];

		yield return [GenerateRemovalTwoPlaceTestCase()];

		yield return [GenerateRemovalDoubleLetterTestCase()];

		yield return [GenerateRemovalLowLetterTestCase()];

		yield return [GenerateRemovalHighLetterTestCase()];

		yield return [GenerateWrongRemovalTestCase()];

		yield return [GenerateMultiplicationTestCase()];

		yield return [GenerateZeroMultiplicationTestCase()];

		yield return [GenerateOperatorCombinationTestCase()];

		yield return [GenerateParenthesesTestCase()];
	}

	private static TestCase GenerateOnlyOperandTestCase()
	{
		return new TestCase
		{
			Expression = "a",
			ExpectedResult = "a",
			ExpectedRpnTokens = ["a"]
		};
	}

	private static TestCase GenerateConcatenationTestCase()
	{
		return new TestCase
		{
			Expression = "a+b",
			ExpectedResult = "ab",
			ExpectedRpnTokens = ["a", "b", "+"]
		};
	}

	private static TestCase GenerateTotalRemovalTestCase()
	{
		return new TestCase
		{
			Expression = "abc-abc",
			ExpectedResult = "",
			ExpectedRpnTokens = ["abc", "abc", "-"]
		};
	}

	private static TestCase GenerateRemovalTwoPlaceTestCase()
	{
		return new TestCase
		{
			Expression = "Sunshine-n",
			ExpectedResult = "Sushie",
			ExpectedRpnTokens = ["Sunshine", "n", "-"]
		};
	}

	private static TestCase GenerateRemovalDoubleLetterTestCase()
	{
		return new TestCase
		{
			Expression = "hello-l",
			ExpectedResult = "heo",
			ExpectedRpnTokens = ["hello", "l", "-"]
		};
	}

	private static TestCase GenerateRemovalLowLetterTestCase()
	{
		return new TestCase
		{
			Expression = "Sunshine-s",
			ExpectedResult = "Sunhine",
			ExpectedRpnTokens = ["Sunshine", "s", "-"]
		};
	}

	private static TestCase GenerateRemovalHighLetterTestCase()
	{
		return new TestCase
		{
			Expression = "Sunshine-S",
			ExpectedResult = "unshine",
			ExpectedRpnTokens = ["Sunshine", "S", "-"]
		};
	}

	private static TestCase GenerateWrongRemovalTestCase()
	{
		return new TestCase
		{
			Expression = "hello-n",
			ExpectedResult = "hello",
			ExpectedRpnTokens = ["hello", "n", "-"]
		};
	}

	private static TestCase GenerateMultiplicationTestCase()
	{
		return new TestCase
		{
			Expression = "Sun*2",
			ExpectedResult = "SunSun",
			ExpectedRpnTokens = ["Sun", "2", "*"]
		};
	}

	private static TestCase GenerateZeroMultiplicationTestCase()
	{
		return new TestCase
		{
			Expression = "a*0",
			ExpectedResult = "",
			ExpectedRpnTokens = ["a", "0", "*"]
		};
	}

	private static TestCase GenerateOperatorCombinationTestCase()
	{
		return new TestCase
		{
			Expression = "Sunshine-n*2",
			ExpectedResult = "Sunshine",
			ExpectedRpnTokens = ["Sunshine", "n", "2", "*", "-"]
		};
	}

	private static TestCase GenerateParenthesesTestCase()
	{
		return new TestCase
		{
			Expression = "T+(Sunshine-n)*2",
			ExpectedResult = "TSushieSushie",
			ExpectedRpnTokens = ["T", "Sunshine", "n", "-", "2", "*", "+"]
		};
	}
}