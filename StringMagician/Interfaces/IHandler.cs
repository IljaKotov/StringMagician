using StringMagician.Core;

namespace StringMagician.Interfaces;

internal interface IHandler
{
	bool IsStopped { get; }
	IAsyncEnumerable<string> ReadInputAsync();
	Task WriteOutputAsync(IEnumerable<ProcessingResult> output);
}