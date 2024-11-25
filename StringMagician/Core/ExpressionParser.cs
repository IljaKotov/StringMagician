using System.Text.RegularExpressions;
using StringMagician.Interfaces;
using StringMagician.Utilities;

namespace StringMagician.Core;

/// <summary>
/// Parses the expression into a list of tokens.
/// </summary>
internal class ExpressionParser : IParser
{
	/// <summary>
	/// Parses the expression into a list of tokens - list of operators and operands. The operators are +, -, *. Operands are any other characters without (,).
	/// </summary>
	/// <param name="expression">The input expression to be parsed.</param>
	/// <returns>A list of tokens extracted from the expression.</returns>
	public async IAsyncEnumerable<string> ParseAsync(string expression)
	{
		Regex regex = RegexPatterns.FullPattern;

		foreach (Match match in regex.Matches(expression))
		{
			await Task.Yield();

			yield return match.Value;
		}
	}
}