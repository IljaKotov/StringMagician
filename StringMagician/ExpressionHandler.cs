using StringMagician.Interfaces;

namespace StringMagician;

public class ExpressionHandler : IExpressionHandler
{
	private readonly OperationFactory _operationFactory;

	public ExpressionHandler(OperationFactory operationFactory)
	{
		_operationFactory = operationFactory;
	}

	public string Handle(string expression)
	{
		var operands = new Stack<string>();
		var operators = new Stack<char>();

		for (var i = 0; i < expression.Length; i++)
		{
			var ch = expression[i];

			if (char.IsLetterOrDigit(ch))
			{
				var operand = string.Empty;

				while (i < expression.Length && char.IsLetterOrDigit(expression[i]))
					operand += expression[i++];

				operands.Push(operand);

				i--; // compensate for the extra increment in the loop
			}
			else if (ch == '(')
			{
				operators.Push(ch);
			}
			else if (ch == ')')
			{
				while (operators.Count > 0 && operators.Peek() != '(')
					ProcessOperation(operands, operators);

				operators.Pop(); // remove '(' from the stack
			}
			else
			{
				while (operators.Count > 0 && HasPrecedence(ch, operators.Peek()))
					ProcessOperation(operands, operators);

				operators.Push(ch);
			}
		}

		while (operators.Count > 0)
			ProcessOperation(operands, operators);

		return operands.Pop();
	}

	private void ProcessOperation(Stack<string> operands, Stack<char> operators)
	{
		var rightOperand = operands.Pop();
		var leftOperand = operands.Pop();
		var operation = _operationFactory.GetOperation(operators.Pop());

		var result = operation.Apply(leftOperand, rightOperand);

		operands.Push(result);
	}

	private static bool HasPrecedence(char operator1, char operator2)
	{
		if (operator2 == '(' || operator2 == ')')
		{
			return false;
		}

		if ((operator1 == '*' || operator1 == '/') && (operator2 == '+' || operator2 == '-'))
		{
			return false;
		}

		return true;
	}
}