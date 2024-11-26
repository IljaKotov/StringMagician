namespace StringMagician.Interfaces;

internal interface IParser
{
	IEnumerable<string> ParseAsync(string expression);
}