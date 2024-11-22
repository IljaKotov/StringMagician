namespace StringMagician.Configuration;

internal record HandlerSettings
{
	public required string InteractiveModeCommand { get; set; }
	public required string FileProcessingModeCommand { get; set; }
	public required string SelectModeMessage { get; set; }
	public required string EnterConsoleTemplate { get; set; }
	public required string EnterFileTemplate { get; set; }
	public required string ResultConsoleMessage { get; set; }
	public required string ErrorConsoleMessage { get; set; }
	public required string OutputFileTemplate { get; set; }
	public required string WriteFileReport { get; set; }
	public required string ErrorFileReport { get; set; }
	public required string ResultFileLineTemplate { get; set; }
	public required string ExitCommand { get; set; }
	public required string FileNonexistent { get; set; }
	public required string GoodBye { get; set; }
}