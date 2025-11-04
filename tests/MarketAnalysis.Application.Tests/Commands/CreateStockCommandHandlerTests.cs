namespace MarketAnalysis.Application.Tests.Commands;

using FluentAssertions;
using Moq;
using Xunit;
using MarketAnalysis.Application.Commands.Stocks;
using MarketAnalysis.Domain.Entities;
using MarketAnalysis.Domain.Repositories;
using Microsoft.Extensions.Logging;

public class CreateStockCommandHandlerTests
{
    private readonly Mock<IStockCommandRepository> _mockRepository;
    private readonly Mock<ILogger<CreateStockCommandHandler>> _mockLogger;
    private readonly CreateStockCommandHandler _handler;

    public CreateStockCommandHandlerTests()
    {
        _mockRepository = new Mock<IStockCommandRepository>();
        _mockLogger = new Mock<ILogger<CreateStockCommandHandler>>();
        _handler = new CreateStockCommandHandler(_mockRepository.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldCreateStock()
    {
        // Arrange
        var command = new CreateStockCommand
        {
            Symbol = "AAPL",
            Name = "Apple Inc.",
            Exchange = "NASDAQ",
            Sector = "Technology",
            Industry = "Consumer Electronics",
            InitialPrice = 150.50m
        };

        _mockRepository.Setup(x => x.GetBySymbolAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Stock?)null);

        _mockRepository.Setup(x => x.AddAsync(It.IsAny<Stock>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBe(Guid.Empty);
        _mockRepository.Verify(x => x.AddAsync(It.IsAny<Stock>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_DuplicateSymbol_ShouldThrowException()
    {
        // Arrange
        var command = new CreateStockCommand
        {
            Symbol = "AAPL",
            Name = "Apple Inc.",
            Exchange = "NASDAQ",
            Sector = "Technology",
            Industry = "Consumer Electronics",
            InitialPrice = 150.50m
        };

        var existingStock = Stock.Create("AAPL", "Apple Inc.", "NASDAQ", "Technology", "Consumer Electronics", 150.50m);
        _mockRepository.Setup(x => x.GetBySymbolAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingStock);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => 
            _handler.Handle(command, CancellationToken.None));
    }
}
