using StringMagician.Configuration;
using StringMagician.Interfaces;

namespace StringMagician.Operations;

/// <summary>
/// Represents a removal operation.
/// </summary>
internal class RemovalOperation : IOperation
{
	private readonly OperationSettings _settings;

	/// <summary>
	/// Gets the priority of the operation.
	/// </summary>
	public int Priority => _settings.Priority;

	/// <summary>
	/// Gets the operator symbol.
	/// </summary>
	public string Operator => _settings.Operator;

	public RemovalOperation(OperationSettings settings)
	{
		_settings = settings;
	}

	/// <summary>
	/// Executes the removal operation - removes all occurrences of the right operand from the left operand.
	/// </summary>
	/// <param name="operandLeft">The string from which to remove.</param>
	/// <param name="operandRight">The string to be removed.</param>
	/// <returns>The resulting string after removal.</returns>
	public string Execute(string operandLeft, string operandRight)
	{
		if (string.IsNullOrEmpty(operandRight) || string.IsNullOrEmpty(operandLeft))
			return operandLeft;

		return operandLeft.Replace(operandRight, string.Empty);
	}
}