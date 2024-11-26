namespace StringMagician.Interfaces;

internal interface IConverter
{
	IEnumerable<string> ConvertToRpn(IEnumerable<string> tokens);
}