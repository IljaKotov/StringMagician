using Microsoft.Extensions.DependencyInjection;
using StringMagician.Configuration;
using StringMagician.Interfaces;

namespace StringMagician.Utilities;

internal class ChainHandlerFactory : IChainHandlerFactory
{
	private readonly IChainHandler _operandHandler;
	private readonly IChainHandler _operatorHandler;
	private readonly IChainHandler _leftParenthesisHandler;
	private readonly IChainHandler _rightParenthesisHandler;

	public ChainHandlerFactory(
		[FromKeyedServices(ChainHandlerType.Operand)]
		IChainHandler operandHandler,
		[FromKeyedServices(ChainHandlerType.Operator)]
		IChainHandler operatorHandler,
		[FromKeyedServices(ChainHandlerType.LeftParenthesis)]
		IChainHandler leftParenthesisHandler,
		[FromKeyedServices(ChainHandlerType.RightParenthesis)]
		IChainHandler rightParenthesisHandler)
	{
		_operandHandler = operandHandler;
		_operatorHandler = operatorHandler;
		_leftParenthesisHandler = leftParenthesisHandler;
		_rightParenthesisHandler = rightParenthesisHandler;
	}

	public IChainHandler CreateChainHandler()
	{
		_operandHandler.SetNext(_operatorHandler)
			.SetNext(_leftParenthesisHandler)
			.SetNext(_rightParenthesisHandler);

		return _operandHandler;
	}
}