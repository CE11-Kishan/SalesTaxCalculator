namespace SalesTaxCalculator.Tests;

using SalesTaxCalculator.Core.Services;
using Xunit;

public class RoundingTests
{
    [Theory]
    [InlineData(0.00, 0.00)]
    [InlineData(0.01, 0.05)]
    [InlineData(0.02, 0.05)]
    [InlineData(0.05, 0.05)]
    [InlineData(0.06, 0.10)]
    [InlineData(0.10, 0.10)]
    [InlineData(1.51, 1.55)]
    [InlineData(1.499, 1.50)]
    [InlineData(1.501, 1.55)]
    [InlineData(2.2485, 2.25)]
    public void RoundUpToNearestFiveCents_RoundsCorrectly(decimal input, decimal expected)
    {
        var result = Rounding.RoundUpToNearestFiveCents(input);
        Assert.Equal(expected, result);
    }
}
