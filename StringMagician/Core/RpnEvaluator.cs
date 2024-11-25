using Microsoft.Extensions.Options;
using StringMagician.Configuration;
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
	private readonly CoreSettings _settings;

	/// <summary>
	/// Initialize a new instance of <see cref="RpnEvaluator"/> with the given operations and context.
	/// </summary>
	/// <param name="operations">The list of operations to be used for evaluation.</param>
	/// <param name="context">The operation context to be used for evaluation.</param>
	/// <param name="coreSettings">The configuration to be used.</param>
	public RpnEvaluator(IEnumerable<IOperation> operations,
		OperationContext context,
		IOptions<CoreSettings> coreSettings)
	{
		_operations = operations.ToDictionary(op => op.Operator, op => op);
		_context = context;
		_settings = coreSettings.Value;
	}

	/// <summary>
	/// Evaluate a list of Reverse Polish Notation tokens to a single value.
	/// </summary>
	/// <param name="rpnTokens">The list of tokens in Reverse Polish Notation.</param>
	/// <returns>The result of the evaluation.</returns>
	/// <exception cref="InvalidOperationException">Thrown when the expression is malformed or there are insufficient operands.</exception>
	public async Task<string> EvaluateAsync(IAsyncEnumerable<string> rpnTokens)
	{
		Stack<string> stack = new();

		await foreach (string token in rpnTokens)
		{
			await ProcessTokenAsync(token, stack);
		}

		ValidateFinalStack(stack);

		return stack.Pop();
	}

	private async Task ProcessTokenAsync(string token, Stack<string> stack)
	{
		if (_operations.TryGetValue(token, out IOperation? operation))
		{
			await ExecuteOperationAsync(stack, operation);
		}
		else
		{
			stack.Push(token);
		}
	}

	private async Task ExecuteOperationAsync(Stack<string> stack, IOperation operation)
	{
		if (stack.Count < _settings.MinimumOperands)
			throw new InvalidOperationException(_settings.InsufficientOperands);

		string operandRight = stack.Pop();
		string operandLeft = stack.Pop();
		_context.SetOperation(operation);
		string result = await Task.Run(() => _context.ExecuteOperation(operandLeft, operandRight));
		stack.Push(result);
	}

	private void ValidateFinalStack(Stack<string> stack)
	{
		if (stack.Count != _settings.FinalStackCount)
			throw new InvalidOperationException(_settings.MalformedExpression);
	}
}