namespace StringMagician.Interfaces;

internal interface IConverter
{
	IAsyncEnumerable<string> ConvertToRpnAsync(IAsyncEnumerable<string> tokens);
}