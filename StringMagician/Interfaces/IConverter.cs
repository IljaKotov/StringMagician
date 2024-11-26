namespace StringMagician.Interfaces;

internal interface IConverter
{
	IEnumerable<string> ConvertToRpnAsync(IEnumerable<string> tokens);
}