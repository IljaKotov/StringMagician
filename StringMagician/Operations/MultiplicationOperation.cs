using StringMagician.Interfaces;

namespace StringMagician.Operations;

public class MultiplicationOperation : IOperation
{
	public char Operator => '*';

	public string Apply(string left, string right)
	{
		if (int.TryParse(right, out var times))
		{
			return string.Concat(Enumerable.Repeat(left, times));
		}

		throw new InvalidOperationException($"Cannot multiply by non-integer: {right}");
	}
}