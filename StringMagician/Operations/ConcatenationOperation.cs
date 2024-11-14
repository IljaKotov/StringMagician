using StringMagician.Interfaces;

namespace StringMagician.Operations;

/// <summary>
/// Represents concatenation operation.
/// </summary>
internal class ConcatenationOperation : IOperation
{
	/// <summary>
	/// Gets the priority of the operation.
	/// </summary>
	public int Priority => 1;

	/// <summary>
	/// Gets the operator symbol.
	/// </summary>
	public string Operator => "+";

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