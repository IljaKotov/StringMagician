using Microsoft.Extensions.Options;
using StringMagician.Configuration;
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
	public static List<IOperation> CreateOperations(IOptions<List<OperationSettings>> operationSettings)
	{
		var settings = operationSettings.Value.ToDictionary(s => s.Id);

		return
		[
			new ConcatenationOperation(settings[OperationSettings.ConcatenationOperationId]),
			new MultiplicationOperation(settings[OperationSettings.MultiplicationOperationId]),
			new RemovalOperation(settings[OperationSettings.RemovalOperationId])
		];
	}
}