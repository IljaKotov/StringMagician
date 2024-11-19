namespace StringMagician.Interfaces;

internal interface IChainHandler
{
	IChainHandler SetNext(IChainHandler handler);

	void Handle(string token,
		List<string> output,
		Stack<string> operators);
}