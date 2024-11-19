namespace StringMagician.Core.Handlers;

internal class LeftParenthesisHandler : AbstractChainHandler
{
	public override void Handle(string token,
		List<string> output,
		Stack<string> operators)
	{
		if (token is LeftParenthesis)
			operators.Push(token);
		else
			base.Handle(token, output, operators);
	}
}