using Microsoft.Extensions.Options;
using StringMagician.Configuration;

namespace StringMagician.Core.Handlers;

internal class RightParenthesisHandler : AbstractChainHandler
{
	private readonly CoreSettings _settings;

	public RightParenthesisHandler(IOptions<CoreSettings> coreSettings)
	{
		_settings = coreSettings.Value;
	}

	public override void Handle(string token,
		List<string> output,
		Stack<string> operators)
	{
		if (token == _settings.RightParenthesis)
		{
			while (operators.Count > _settings.EmptyStack && operators.Peek() != _settings.LeftParenthesis)
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