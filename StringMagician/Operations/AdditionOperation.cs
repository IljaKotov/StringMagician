using StringMagician.Interfaces;

namespace StringMagician.Operations;

public class AdditionOperation : IOperation
{
	public char Operator => '+';

	public string Apply(string left, string right)
	{
		return left + right;
	}
}