namespace StringMagician.Interfaces;

internal interface IParser
{
	IAsyncEnumerable<string> ParseAsync(string expression);
}