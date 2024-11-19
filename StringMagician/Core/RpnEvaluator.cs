using StringMagician.Interfaces;
using StringMagician.Operations;

namespace StringMagician.Core;

/// <summary>
/// Evaluate a list of tokens to result in a single value.
/// </summary>
internal class RpnEvaluator : IEvaluator
{
	private readonly IDictionary<string, IOperation> _operations;
    private readonly OperationContext _context;

    
    /// <summary>
    /// Initialize a new instance of <see cref="RpnEvaluator"/> with the given operations and context.
    /// </summary>
    /// <param name="operations">The list of operations to be used for evaluation.</param>
    /// <param name="context">The operation context to be used for evaluation.</param>
    public RpnEvaluator(IEnumerable<IOperation> operations, OperationContext context)
    {
        _operations = operations.ToDictionary(op => op.Operator, op => op);
        _context = context;
    }

    /// <summary>
    /// Evaluate a list of Reverse Polish Notation tokens to a single value.
    /// </summary>
    /// <param name="rpnTokens">The list of tokens in Reverse Polish Notation.</param>
    /// <returns>The result of the evaluation.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the expression is malformed or there are insufficient operands.</exception>
    public string Evaluate(List<string> rpnTokens)
    {
        var stack = new Stack<string>();

        foreach (var token in rpnTokens)
        {
            ProcessToken(token, stack);
        }

        ValidateFinalStack(stack);

        return stack.Pop();
    }

    private void ProcessToken(string token, Stack<string> stack)
    {
        if (_operations.TryGetValue(token, out var operation))
        {
            ExecuteOperation(stack, operation);
        }
        else
        {
            stack.Push(token);
        }
    }

    private void ExecuteOperation(Stack<string> stack, IOperation operation)
    {
        if (stack.Count < 2)
            throw new InvalidOperationException("Insufficient operands.");

        var operandRight = stack.Pop();
        var operandLeft = stack.Pop();
        _context.SetOperation(operation);
        var result = _context.ExecuteOperation(operandLeft, operandRight);
        stack.Push(result);
    }

    private static void ValidateFinalStack(Stack<string> stack)
    {
        if (stack.Count != 1)
            throw new InvalidOperationException("Malformed expression.");
    }
}