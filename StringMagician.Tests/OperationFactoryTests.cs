using StringMagician.Operations;

namespace StringMagician.Tests;

public class OperationFactoryTests
{
	private readonly OperationFactory _operationFactory = new();

	[Fact]
	public void GetOperation_AdditionSymbol_ReturnsAdditionOperation()
	{
		var operation = _operationFactory.GetOperation('+');
		Assert.IsType<AdditionOperation>(operation);
	}

	[Fact]
	public void GetOperation_SubtractionSymbol_ReturnsSubtractionOperation()
	{
		var operation = _operationFactory.GetOperation('-');
		Assert.IsType<SubtractionOperation>(operation);
	}

	[Fact]
	public void GetOperation_MultiplicationSymbol_ReturnsMultiplicationOperation()
	{
		var operation = _operationFactory.GetOperation('*');
		Assert.IsType<MultiplicationOperation>(operation);
	}

	[Fact]
	public void GetOperation_InvalidSymbol_ThrowsInvalidOperationException()
	{
		var exception = Assert.Throws<InvalidOperationException>(() => _operationFactory.GetOperation('/'));
		Assert.Equal("Unsupported operator: /", exception.Message);
	}
}