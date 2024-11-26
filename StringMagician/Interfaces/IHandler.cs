using StringMagician.Core;

namespace StringMagician.Interfaces;

internal interface IHandler
{
	IAsyncEnumerable<string> ReadInputAsync(CancellationToken cancellationToken);
	Task WriteOutputAsync(IEnumerable<ProcessingResult> output, CancellationToken cancellationToken);
}