using System.Text;
using StringMagician.Configuration;
using StringMagician.Interfaces;

namespace StringMagician.Operations;

/// <summary>
/// Represents a multiplication operation.
/// </summary>
internal class MultiplicationOperation : IOperation
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

	public MultiplicationOperation(OperationSettings settings)
	{
		_settings = settings;
	}

	/// <summary>
	/// Executes the multiplication operation - repeats the string a specified number of times.
	/// </summary>
	/// <param name="operandLeft">The string to be repeated.</param>
	/// <param name="operandRight">The number of times to repeat the string.</param>
	/// <returns>The concatenated string after repetition.</returns>
	/// <exception cref="ArgumentException">Thrown when the multiplier is not a non-negative integer.</exception>
	public string Execute(string operandLeft, string operandRight)
	{
		bool isValidNumber = int.TryParse(operandRight, out int repeatCount);
		bool isNonNegative = repeatCount >= 0;

		if (isValidNumber is false || isNonNegative is false)
			throw new ArgumentException("Multiplier should be a non-negative integer.");

		StringBuilder result = new();

		for (int i = 0; i < repeatCount; i++)
			result.Append(operandLeft);

		return result.ToString();
	}
}