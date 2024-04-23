using StringMagician.Interfaces;

namespace StringMagician.Operations;

public class SubtractionOperation : IOperation
{
	public char Operator => '-';

	public string Apply(string left, string right)
	{
		return left.Replace(right, "");
	}
}