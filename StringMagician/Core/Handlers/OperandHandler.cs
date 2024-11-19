using StringMagician.Utilities;

namespace StringMagician.Core.Handlers;

internal class OperandHandler: AbstractChainHandler
{
	public override void Handle(string token, List<string> output, Stack<string> operators)
	{
		if (RegexPatterns.OperandPattern.IsMatch(token))
			output.Add(token);
		else
			base.Handle(token, output, operators);
	}
}