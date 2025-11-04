namespace MarketAnalysis.Domain.Tests.Entities;

using FluentAssertions;
using MarketAnalysis.Domain.Entities;
using Xunit;

public class StockTests
{
    [Fact]
    public void Create_ValidStock_ShouldSucceed()
    {
        // Arrange & Act
        var stock = Stock.Create(
            "AAPL",
            "Apple Inc.",
            "NASDAQ",
            "Technology",
            "Consumer Electronics",
            150.50m);

        // Assert
        stock.Should().NotBeNull();
        stock.Symbol.Value.Should().Be("AAPL");
        stock.Name.Should().Be("Apple Inc.");
        stock.CurrentPrice.Value.Should().Be(150.50m);
        stock.IsActive.Should().BeTrue();
    }

    [Fact]
    public void UpdatePrice_ValidPrice_ShouldUpdateSuccessfully()
    {
        // Arrange
        var stock = Stock.Create("AAPL", "Apple Inc.", "NASDAQ", "Technology", "Consumer Electronics", 150.50m);
        
        // Act
        stock.UpdatePrice(160.75m);

        // Assert
        stock.CurrentPrice.Value.Should().Be(160.75m);
        stock.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public void UpdatePrice_NegativePrice_ShouldThrowException()
    {
        // Arrange
        var stock = Stock.Create("AAPL", "Apple Inc.", "NASDAQ", "Technology", "Consumer Electronics", 150.50m);
        
        // Act & Assert
        Assert.Throws<ArgumentException>(() => stock.UpdatePrice(-10m));
    }

    [Fact]
    public void Deactivate_ShouldSetIsActiveToFalse()
    {
        // Arrange
        var stock = Stock.Create("AAPL", "Apple Inc.", "NASDAQ", "Technology", "Consumer Electronics", 150.50m);
        
        // Act
        stock.Deactivate();

        // Assert
        stock.IsActive.Should().BeFalse();
        stock.UpdatedAt.Should().NotBeNull();
    }
}
