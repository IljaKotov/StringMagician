using StringMagician.Core;

namespace StringMagician.Interfaces;

internal interface IProcessorCore
{
	ProcessingResult ProcessLine(string line);
}