using StringMagician.Interfaces;

namespace StringMagician.Core;

/// <summary>
/// Core processor for parsing, converting and evaluating expressions.
/// </summary>
internal class ProcessorCore : IProcessorCore
{
	private readonly IParser _parser;
	private readonly IConverter _converter;
	private readonly IEvaluator _evaluator;

	/// <summary>
	/// Initializes a new instance of the <see cref="ProcessorCore"/> class.
	/// </summary>
	/// <param name="parser">The parser to be used for parsing expressions.</param>
	/// <param name="converter">The converter to be used for converting expressions to RPN.</param>
	/// <param name="evaluator">The evaluator to be used for evaluating RPN expressions.</param>
	internal ProcessorCore(IParser parser,
		IConverter converter,
		IEvaluator evaluator)
	{
		_parser = parser;
		_converter = converter;
		_evaluator = evaluator;
	}

	/// <summary>
	/// Processes a line of input by parsing, converting, and evaluating it.
	/// </summary>
	/// <param name="line">The input line to be processed.</param>
	/// <returns>The result of the processing.</returns>
	public string ProcessLine(string line)
	{
		try
		{
			var tokens = _parser.Parse(line);

			var rpnTokens = _converter.ConvertToRpn(tokens);

			var result = _evaluator.Evaluate(rpnTokens);

			return $"Original operation: {line}\nResult: {result}";
		}
		catch (Exception ex)
		{
			return $"Original operation: {line}\nResult: Error - {ex.Message}";
		}
	}
}