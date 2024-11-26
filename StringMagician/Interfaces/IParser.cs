namespace StringMagician.Interfaces;

internal interface IParser
{
	IEnumerable<string> Parse(string expression);
}