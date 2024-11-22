using Microsoft.Extensions.Options;
using StringMagician.Configuration;

namespace StringMagician.Core.Handlers;

internal class LeftParenthesisHandler : AbstractChainHandler
{
	private readonly CoreSettings _settings;

	public LeftParenthesisHandler(IOptions<CoreSettings> coreSettings)
	{
		_settings = coreSettings.Value;
	}

	public override void Handle(string token,
		List<string> output,
		Stack<string> operators)
	{
		if (token == _settings.LeftParenthesis)
			operators.Push(token);
		else
			base.Handle(token, output, operators);
	}
}