using StringMagician.Interfaces;
using StringMagician.Utilities;

namespace StringMagician.Core;

/// <summary>
/// Converts infix notation to reverse polish notation.
/// </summary>
internal class RpnConverter : IConverter
{
	private const string LeftParenthesis = "(";
	private const string RightParenthesis = ")";
	private const int EmptyStack = 0;
	private readonly Stack<string> _operators;
	private readonly Dictionary<string, int> _operationPriorities;

	/// <summary>
	/// Initializes a new instance of the <see cref="RpnConverter"/> class.
	/// </summary>
	/// <param name="operations">The list of operations to be used for conversion.</param>
	public RpnConverter(IEnumerable<IOperation> operations)
	{
		_operators = new Stack<string>();

		_operationPriorities = operations.ToDictionary(
			op => op.Operator,
			op => op.Priority);
	}

	/// <summary>
	/// Converts a list of tokens to RPN.
	/// </summary>
	/// <param name="tokens">List of operands, parenthesis and operators in infix notation.</param>
	/// <returns>A list of tokens in reverse polish notation.</returns>
	public List<string> ConvertToRpn(IEnumerable<string> tokens)
	{
		var output = new List<string>();
		foreach (var token in tokens)
		{
			if (IsOperand(token))
				output.Add(token);

			if (IsOperator(token))
				ProcessOperators(token, output);

			if (token is LeftParenthesis)
				_operators.Push(token);

			if (token is RightParenthesis)
				CloseParenthesis(output);
		}

		while (_operators.Count is not EmptyStack)
			output.Add(_operators.Pop());

		return output;
	}

	private int GetPriority(string op)
	{
		return _operationPriorities.TryGetValue(op, out var priority)
			? priority
			: throw new InvalidOperationException($"Unknown operation: {op}");
	}

	private static bool IsOperand(string token)
	{
		return RegexPatterns.OperandPattern.IsMatch(token);
	}

	private static bool IsOperator(string token)
	{
		return RegexPatterns.OperatorPattern.IsMatch(token);
	}

	private void ProcessOperators(string token, List<string> output)
	{
		while (_operators.Count is not EmptyStack && _operators.Peek() is not LeftParenthesis &&
			GetPriority(_operators.Peek()) >= GetPriority(token))
		{
			output.Add(_operators.Pop());
		}

		_operators.Push(token);
	}

	private void CloseParenthesis( List<string> output)
	{
		while (_operators.Count is not EmptyStack && _operators.Peek() is not LeftParenthesis)
			output.Add(_operators.Pop());

		_operators.Pop();
	}
}