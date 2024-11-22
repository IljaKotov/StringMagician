namespace StringMagician.Configuration;

internal record OperationSettings
{
	public const string ConcatenationOperationId = "Concatenation";
	public const string MultiplicationOperationId = "Multiplication";
	public const string RemovalOperationId = "Removal";
	public required string Id { get; init; }
	public required string Operator { get; init; }
	public required int Priority { get; init; }
}