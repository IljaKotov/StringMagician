using StringMagician.Configuration;
using StringMagician.Interfaces;

namespace StringMagician.Operations;

/// <summary>
/// Represents concatenation operation.
/// </summary>
internal class ConcatenationOperation : IOperation
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

	public ConcatenationOperation(OperationSettings settings)
	{
		_settings = settings;
	}

	/// <summary>
	/// Executes the concatenation operation - concatenates two strings.
	/// </summary>
	/// <param name="operandLeft">The left operand in expression.</param>
	/// <param name="operandRight">The right operand in expression.</param>
	/// <returns>The concatenated string.</returns>
	public string Execute(string operandLeft, string operandRight)
	{
		return $"{operandLeft}{operandRight}";
	}
}