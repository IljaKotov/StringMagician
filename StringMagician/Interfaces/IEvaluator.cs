namespace StringMagician.Interfaces;

internal interface IEvaluator
{
	string Evaluate(IEnumerable<string> rpnTokens);
}