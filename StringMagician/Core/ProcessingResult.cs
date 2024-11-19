namespace StringMagician.Core;

public record ProcessingResult(
	string OriginalOperation,
	string Result,
	string ErrorMessage);