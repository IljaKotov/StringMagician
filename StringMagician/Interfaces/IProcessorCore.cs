using StringMagician.Core;

namespace StringMagician.Interfaces;

internal interface IProcessorCore
{
	Task<ProcessingResult> ProcessLine(string line);
}