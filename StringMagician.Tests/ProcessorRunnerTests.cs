using StringMagician.Interfaces;
using NSubstitute;
using StringMagician.Core;

namespace StringMagician.Tests;

public class ProcessorRunnerTests
{
	[Fact]
	public async Task RunAsync_ShouldProcessLinesCorrectly()
	{
		IProcessorCore? mockProcessorCore = Substitute.For<IProcessorCore>();
		IHandler? mockHandler = Substitute.For<IHandler>();
		IHandlerFactory? mockHandlerFactory = Substitute.For<IHandlerFactory>();

		mockHandler.ReadInputAsync(Arg.Any<CancellationToken>()).Returns(GetTestLines());

		mockHandler.WriteOutputAsync(Arg.Any<IEnumerable<ProcessingResult>>(), Arg.Any<CancellationToken>())
			.Returns(Task.CompletedTask);

		mockProcessorCore.ProcessLine(Arg.Any<string>())
			.Returns(new ProcessingResult("Line", "Processed", string.Empty));

		mockHandlerFactory.SelectHandler().Returns(mockHandler);

		ProcessorRunner runner = new(mockProcessorCore, mockHandlerFactory);

		using CancellationTokenSource cts = new();

		await runner.RunAsync(cts.Token);

		await foreach (string line in GetTestLines().WithCancellation(cts.Token))
		{
			mockProcessorCore.Received().ProcessLine(line);
		}

		mockProcessorCore.Received(3).ProcessLine(Arg.Any<string>());

		await mockHandler.Received(3)
			.WriteOutputAsync(
				Arg.Is<IEnumerable<ProcessingResult>>(r => r.All(result => result.Result == "Processed")),
				Arg.Any<CancellationToken>());
	}

	private static async IAsyncEnumerable<string> GetTestLines()
	{
		yield return "Line1";
		yield return "Line2";
		yield return "Line3";

		await Task.CompletedTask;
	}
}