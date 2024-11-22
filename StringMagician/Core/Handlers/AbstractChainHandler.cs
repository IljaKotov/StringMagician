using StringMagician.Interfaces;

namespace StringMagician.Core.Handlers;

internal abstract class AbstractChainHandler : IChainHandler
{
	private IChainHandler? _nextHandler;

	public IChainHandler SetNext(IChainHandler handler)
	{
		_nextHandler = handler;

		return handler;
	}

	public virtual void Handle(string token,
		List<string> output,
		Stack<string> operators)
	{
		ArgumentNullException.ThrowIfNull(_nextHandler);
		_nextHandler.Handle(token, output, operators);
	}
}