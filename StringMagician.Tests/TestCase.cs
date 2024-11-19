namespace StringMagician.Tests;

public class TestCase
{
	public string Expression { get; init; } = string.Empty;
	public string ExpectedResult { get; init; } = string.Empty;
	public string[] ExpectedRpnTokens { get; init; } = [];
}