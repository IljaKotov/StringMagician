namespace StringMagician.Interfaces;

internal interface IOperationContext
{
	void SetOperation(IOperation operation);
	string ExecuteOperation(string operandLeft, string operandRight);
}