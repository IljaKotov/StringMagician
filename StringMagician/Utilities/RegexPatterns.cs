using System.Text.RegularExpressions;

namespace StringMagician.Utilities;

/// <summary>
/// Contains regular expression patterns for parsing expressions to tokens (operands and operators).
/// </summary>
internal record RegexPatterns
{
	public static readonly Regex OperandPattern = new(@"[^+\-()*]+", RegexOptions.Compiled);
	public static readonly Regex OperatorPattern = new(@"[+*-]", RegexOptions.Compiled);
	public static readonly Regex FullPattern = new(@"[^+\-()*]+|[+*()-]", RegexOptions.Compiled);
}