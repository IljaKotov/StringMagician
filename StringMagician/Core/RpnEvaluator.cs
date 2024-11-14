using StringMagician.Interfaces;

namespace StringMagician.Core;

/// <summary>
/// Evaluate a list of tokens to result in a single value.
/// </summary>
internal class RpnEvaluator : IEvaluator
{
	private readonly IDictionary<string, IOperation> _operations;

	/// <summary>
	/// Initialize a new instanse of <see cref="RpnEvaluator"/> with the given operations.
	/// </summary>
	/// <param name="operations">The list of operations to be used for evaluation.</param>
	internal RpnEvaluator(IEnumerable<IOperation> operations)
	{
		_operations = operations.ToDictionary(op => op.Operator, op => op);
	}

	/// <summary>
	/// Evaluate a list of RPN tokens to a single value.
	/// </summary>
	/// <param name="rpnTokens">The list of tokens in RPN.</param>
	/// <returns>The result of the evaluation.</returns>
	/// <exception cref="InvalidOperationException">Thrown when the expression is malformed or there are insufficient operands.</exception>
	public string Evaluate(List<string> rpnTokens)
	{
		var stack = new Stack<string>();

		foreach (var token in rpnTokens)
		{
			if (_operations.TryGetValue(token, out var value))
			{
				if (stack.Count < 2)
					throw new InvalidOperationException("Insufficient operands.");

				var operandRight = stack.Pop();
				var operandLeft = stack.Pop();
				var result = value.Execute(operandLeft, operandRight);
				stack.Push(result);
			}
			else
			{
				stack.Push(token);
			}
		}

		if (stack.Count is not 1)
			throw new InvalidOperationException("Malformed expression.");

		return stack.Pop();
	}
}