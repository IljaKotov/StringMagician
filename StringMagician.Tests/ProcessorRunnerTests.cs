using StringMagician.Interfaces;
using NSubstitute;
using StringMagician.Core;

namespace StringMagician.Tests;

public class ProcessorRunnerTests
{
	[Fact]
	public async Task RunAsync_ShouldProcessLinesCorrectly()
	{
		// Arrange
		var mockProcessorCore = Substitute.For<IProcessorCore>();
		var mockHandler = Substitute.For<IHandler>();
		var mockHandlerFactory = Substitute.For<IHandlerFactory>();

		mockHandler.ReadInputAsync().Returns(GetTestLines());
		mockHandler.WriteOutputAsync(Arg.Any<IEnumerable<ProcessingResult>>()).Returns(Task.CompletedTask);

		mockProcessorCore.ProcessLine(Arg.Any<string>())
			.Returns(Task.FromResult(new ProcessingResult("Line", "Processed", string.Empty)));

		mockHandlerFactory.SelectHandler().Returns(mockHandler);
		
		var runner = new ProcessorRunner(mockProcessorCore, mockHandlerFactory);

		// Act
		await runner.RunAsync();

		// Assert
		await foreach (var line in GetTestLines())
		{
			await mockProcessorCore.Received().ProcessLine(line);
		}

		await mockProcessorCore.Received(3).ProcessLine(Arg.Any<string>());

		await mockHandler.Received(3)
			.WriteOutputAsync(
				Arg.Is<IEnumerable<ProcessingResult>>(r => r.All(result => result.Result == "Processed")));
	}

	private static async IAsyncEnumerable<string> GetTestLines()
	{
		yield return "Line1";
		yield return "Line2";
		yield return "Line3";

		await Task.CompletedTask;
	}
}