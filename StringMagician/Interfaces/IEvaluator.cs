namespace StringMagician.Interfaces;

internal interface IEvaluator
{
	string Evaluate(List<string> rpnTokens);
}