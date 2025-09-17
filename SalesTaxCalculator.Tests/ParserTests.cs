namespace SalesTaxCalculator.Tests;

using SalesTaxCalculator.Core.Services;
using Xunit;

public class ParserTests
{
    [Theory]
    [InlineData("1 book at 12.49", 1, "book", 12.49)]
    [InlineData("2 chocolate bars at 0.85", 2, "chocolate bars", 0.85)]
    [InlineData("1 imported bottle of perfume at 47.50", 1, "imported bottle of perfume", 47.50)]
    public void Parse_ValidInput_ReturnsCorrectBasketItem(string input, int expectedQuantity, string expectedName, decimal expectedPrice)
    {
        var item = InputLineParser.Parse(input);
        
        Assert.Equal(expectedQuantity, item.Quantity);
        Assert.Equal(expectedName, item.Product.Name);
        Assert.Equal(expectedPrice, item.Product.UnitPrice);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("invalid line")]
    [InlineData("1 book")]
    [InlineData("book at 12.49")]
    [InlineData("0 book at 12.49")]
    [InlineData("-1 book at 12.49")]
    [InlineData("1 book at -12.49")]
    [InlineData("abc book at 12.49")]
    [InlineData("1 book at abc")]
    public void Parse_InvalidInput_ThrowsFormatException(string input)
    {
        Assert.Throws<FormatException>(() => InputLineParser.Parse(input));
    }
    
    [Fact]
    public void Parse_NormalizesWhitespace()
    {
        var item = InputLineParser.Parse("1   imported    book   at   12.49");
        Assert.Equal("imported book", item.Product.Name);
    }
}
