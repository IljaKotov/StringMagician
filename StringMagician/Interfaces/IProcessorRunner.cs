namespace StringMagician.Interfaces;

internal interface IProcessorRunner
{
	Task RunAsync(CancellationToken cancellationToken);
}