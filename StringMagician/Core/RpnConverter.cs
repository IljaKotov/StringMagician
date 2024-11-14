using System.Text.RegularExpressions;
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

	private readonly List<string> _output;
	private readonly Dictionary<string, int> _operationPriorities;

	/// <summary>
	/// Initializes a new instance of the <see cref="RpnConverter"/> class.
	/// </summary>
	/// <param name="operations">The list of operations to be used for conversion.</param>
	public RpnConverter(IEnumerable<IOperation> operations)
	{
		_output = [];

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
		_output.Clear();
		var operators = new Stack<string>();

		foreach (var token in tokens)
		{
			if (IsOperand(token))
				_output.Add(token);

			if (IsOperator(token))
				ProcessOperators(operators, token);

			if (token is LeftParenthesis)
				operators.Push(token);

			if (token is RightParenthesis)
				CloseParenthesis(operators, _output);
		}

		while (operators.Count is not EmptyStack)
			_output.Add(operators.Pop());

		return _output;
	}

	private int GetPriority(string op)
	{
		return _operationPriorities.TryGetValue(op, out var priority)
			? priority
			: throw new InvalidOperationException($"Unknown operation: {op}");
	}

	private static bool IsOperand(string token)
	{
		return Regex.IsMatch(token, RegexPatterns.OperandPattern);
	}

	private static bool IsOperator(string token)
	{
		return Regex.IsMatch(token, RegexPatterns.OperatorPattern);
	}

	private void ProcessOperators(Stack<string> operators, string token)
	{
		while (operators.Count is not EmptyStack && operators.Peek() is not LeftParenthesis &&
			GetPriority(operators.Peek()) >= GetPriority(token))
		{
			_output.Add(operators.Pop());
		}

		operators.Push(token);
	}

	private static void CloseParenthesis(Stack<string> operators, List<string> output)
	{
		while (operators.Count is not EmptyStack && operators.Peek() is not LeftParenthesis)
			output.Add(operators.Pop());

		operators.Pop();
	}
}