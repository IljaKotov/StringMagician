namespace StringMagician.Interfaces;

internal interface IConverter
{
	List<string> ConvertToRpn(IEnumerable<string> tokens);
}