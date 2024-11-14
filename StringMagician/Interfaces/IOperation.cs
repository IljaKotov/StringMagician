namespace StringMagician.Interfaces;

internal interface IOperation
{
	int Priority { get; }
	string Operator { get; }
	string Execute(string operandLeft, string operandRight);
}