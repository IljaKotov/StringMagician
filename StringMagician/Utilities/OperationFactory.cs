using StringMagician.Interfaces;
using StringMagician.Operations;

namespace StringMagician.Utilities;

/// <summary>
/// Provides a factory method to create a list of operations.
/// </summary>
internal static class OperationFactory
{
	/// <summary>
	/// Creates a list of available operations.
	/// </summary>
	/// <returns>A list of <see cref="IOperation"/> instances.</returns>
	public static List<IOperation> CreateOperations()
	{
		return
		[
			new ConcatenationOperation(),
			new MultiplicationOperation(),
			new RemovalOperation()
		];
	}
}