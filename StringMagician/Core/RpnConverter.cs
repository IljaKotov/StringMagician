using StringMagician.Interfaces;

namespace StringMagician.Core;

/// <summary>
/// Converts infix notation to reverse polish notation.
/// </summary>
internal class RpnConverter : IConverter
{
	private readonly IChainHandler _chain;

	/// <summary>
	/// Initializes a new instance of the <see cref="RpnConverter"/> class.
	/// </summary>
	/// <param name="chain">The chain of handlers to be used for conversion.</param>
	public RpnConverter(IChainHandler chain)
	{
		_chain = chain;
	}

	/// <summary>
	/// Converts a list of tokens to Reverse Polish Notation.
	/// </summary>
	/// <param name="tokens">List of operands, parenthesis and operators in infix notation.</param>
	/// <returns>A list of tokens in Reverse Polish Notation.</returns>
	public async IAsyncEnumerable<string> ConvertToRpnAsync(IAsyncEnumerable<string> tokens)
	{
		List<string> output = [];
		Stack<string> operators = new();

		await foreach (string token in tokens)
		{
			_chain.Handle(token, output, operators);
		}

		while (operators.Count > 0)
		{
			output.Add(operators.Pop());
		}

		foreach (string item in output)
		{
			yield return item;
		}
	}
}