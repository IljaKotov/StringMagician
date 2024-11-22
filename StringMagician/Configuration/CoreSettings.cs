namespace StringMagician.Configuration;

internal record CoreSettings
{
	public required string InsufficientOperands { get; init; }
	public required string MalformedExpression { get; init; }
	public required int FinalStackCount { get; init; }
	public required int MinimumOperands { get; init; }
	public required string LeftParenthesis { get; init; }
	public required string RightParenthesis { get; init; }
	public required int EmptyStack { get; init; }
}