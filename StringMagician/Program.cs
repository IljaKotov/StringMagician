using System.Text;
Console.WriteLine("Enter 'Stop' to exit the program");
while (Console.ReadLine() != "Stop")
{

	Console.WriteLine("Enter the expression:");
	
	var stringExpression = Console.ReadLine();

	var operands = new Stack<string>();
	var operators = new Stack<char>();

	ArgumentException.ThrowIfNullOrWhiteSpace(stringExpression);

	for (int i = 0; i < stringExpression.Length; i++)
	{
		if (stringExpression[i] != '+' && stringExpression[i] != '-' && stringExpression[i] != '*' && stringExpression[i] != '(' && stringExpression[i] != ')')
		{
			var operand = new StringBuilder();

			while (i < stringExpression.Length &&
				(char.IsLetter(stringExpression[i]) || char.IsDigit(stringExpression[i])))
			{
				operand.Append(stringExpression[i]);
				i++;
			}

			operands.Push(operand.ToString());
			i--;
		}
		else if (stringExpression[i] == '(')
		{
			operators.Push(stringExpression[i]);
		}
		else if (stringExpression[i] == ')')
		{
			while (operators.Count > 0 && operators.Peek() != '(')
			{
				PerformOperation(operands, operators);
			}

			if (operators.Count > 0)
			{
				operators.Pop();
			}
		}
		else if (stringExpression[i] == '+' || stringExpression[i] == '-' || stringExpression[i] == '*')
		{
			while (operators.Count > 0 && Priority(operators.Peek()) >= Priority(stringExpression[i]))
			{
				PerformOperation(operands, operators);
			}

			operators.Push(stringExpression[i]);
		}
	}

	while (operators.Count > 0)
	{
		PerformOperation(operands, operators);
	}

	Console.WriteLine($"{stringExpression} = {operands.Pop()}");
}

return;

static void PerformOperation(Stack<string> operands, Stack<char> operators)
{
    string result;
    var action = operators.Pop();

    switch (action)
    {
        case '+':
			var secondOperand = operands.Pop();
			var firstOperand = operands.Pop();
			result = firstOperand + secondOperand;
			break;
        case '-':
            var toRemove = operands.Pop();
            result = operands.Pop().Replace(toRemove, "");
            break;
        case '*':
            var timesOperand = operands.Pop();
            if (int.TryParse(timesOperand, out var times))
            {
                result = string.Concat(Enumerable.Repeat(operands.Pop(), times));
            }
            else
            {
                throw new InvalidOperationException($"Cannot multiply by non-integer: {timesOperand}");
            }
            break;
        default:
            throw new InvalidOperationException($"Unknown operator: {action}");
    }

    operands.Push(result);
}

static int Priority(char op)
{
    switch (op)
    {
        case '+':
        case '-':
            return 1;
        case '*':
            return 2;
        default:
            return 0;
    }
}