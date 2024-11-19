using StringMagician.Core;

namespace StringMagician.Interfaces;

internal interface IHandler
{
	bool IsStopped { get; }
	IEnumerable<string> ReadInput();
	void WriteOutput(IEnumerable<ProcessingResult> output);
}