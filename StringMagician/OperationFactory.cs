using StringMagician.Interfaces;
using StringMagician.Operations;

namespace StringMagician;

public class OperationFactory
{
	private readonly Dictionary<char, IOperation> _operations = new()
	{
		{
			'+', new AdditionOperation()
		},
		{
			'-', new SubtractionOperation()
		},
		{
			'*', new MultiplicationOperation()
		}
	};

	public IOperation GetOperation(char operatorSymbol)
	{
		if (_operations.TryGetValue(operatorSymbol, out var operation))
		{
			return operation;
		}

		throw new InvalidOperationException($"Unsupported operator: {operatorSymbol}");
	}
}