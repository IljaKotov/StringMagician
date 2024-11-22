using Microsoft.Extensions.Options;
using StringMagician.Configuration;
using StringMagician.Utilities;

namespace StringMagician.Core.Handlers;

internal class OperatorHandler : AbstractChainHandler
{
	private readonly IDictionary<string, int> _operationPriorities;
	private readonly CoreSettings _settings;

	public OperatorHandler(IDictionary<string, int> operationPriorities, IOptions<CoreSettings> coreSettings)
	{
		_operationPriorities = operationPriorities;
		_settings = coreSettings.Value;
	}

	public override void Handle(string token,
		List<string> output,
		Stack<string> operators)
	{
		if (RegexPatterns.OperatorPattern.IsMatch(token))
		{
			while (operators.Count > 0 && operators.Peek() != _settings.LeftParenthesis &&
				_operationPriorities[operators.Peek()] >= _operationPriorities[token])
			{
				output.Add(operators.Pop());
			}

			operators.Push(token);
		}
		else
		{
			base.Handle(token, output, operators);
		}
	}
}