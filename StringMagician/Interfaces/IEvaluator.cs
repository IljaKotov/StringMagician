namespace StringMagician.Interfaces;

internal interface IEvaluator
{
	string EvaluateAsync(IEnumerable<string> rpnTokens);
}