namespace StringMagician.Core.Handlers;

internal class RightParenthesisHandler: AbstractChainHandler
{
	public override void Handle(string token, List<string> output, Stack<string> operators)
	{
		if (token is RightParenthesis)
		{
			while (operators.Count > EmptyStack && operators.Peek() is not LeftParenthesis)
			{
				output.Add(operators.Pop());
			}
			operators.Pop();
		}
		else
		{
			base.Handle(token, output, operators);
		}
	}
}