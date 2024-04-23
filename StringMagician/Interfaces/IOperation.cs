namespace StringMagician.Interfaces;

public interface IOperation
{
	char Operator { get; }
	string Apply(string left, string right);
}