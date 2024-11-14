namespace StringMagician.Utilities;

/// <summary>
/// Contains regular expression patterns for parsing expressions to tokens (operands and operators).
/// </summary>
internal class RegexPatterns
{
	public const string OperandPattern = @"[^+\-()*]+";
	public const string OperatorPattern = @"[+*-]";
	public const string FullPattern = @"[^+\-()*]+|[+*()-]";
}