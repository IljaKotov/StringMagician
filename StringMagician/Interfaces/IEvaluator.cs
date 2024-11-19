namespace StringMagician.Interfaces;

internal interface IEvaluator
{
	Task<string> EvaluateAsync(IAsyncEnumerable<string> rpnTokens);
}