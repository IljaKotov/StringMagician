using StringMagician.Interfaces;

namespace StringMagician.Operations;

internal class OperationContext : IOperationContext
{
	private IOperation? _operation;

	public void SetOperation(IOperation operation)
	{
		_operation = operation;
	}

	public string ExecuteOperation(string operandLeft, string operandRight)
	{
		ArgumentNullException.ThrowIfNull(_operation);

		return _operation.Execute(operandLeft, operandRight);
	}
}