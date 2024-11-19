using StringMagician.Interfaces;

namespace StringMagician.Core.Handlers;

internal abstract class AbstractChainHandler: IChainHandler
{
	private IChainHandler _nextHandler;
	protected const string LeftParenthesis = "(";
	protected const string RightParenthesis = ")";
	protected const int EmptyStack = 0;
	
	public IChainHandler SetNext(IChainHandler handler)
	{
		_nextHandler = handler;
		return handler;
	}
	
	public virtual void Handle(string token, List<string> output, Stack<string> operators)
	{
		_nextHandler.Handle(token, output, operators);
	}
}