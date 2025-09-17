namespace SalesTaxCalculator.Tests;

using SalesTaxCalculator.Core.Models;
using SalesTaxCalculator.Core.Services;
using Xunit;

public class ReceiptTests
{
    private Receipt Build(params string[] lines)
    {
        var basket = new Basket();
        foreach (var line in lines)
        {
            var item = InputLineParser.Parse(line);
            basket.Add(item);
        }
        return basket.GenerateReceipt();
    }

    [Fact]
    public void Basket1()
    {
        var receipt = Build(
            "1 book at 12.49",
            "1 music CD at 14.99",
            "1 chocolate bar at 0.85");
        Assert.Equal(1.50m, receipt.TotalTaxes);
        Assert.Equal(29.83m, receipt.Total);
    }

    [Fact]
    public void Basket2()
    {
        var receipt = Build(
            "1 imported box of chocolates at 10.00",
            "1 imported bottle of perfume at 47.50");
        Assert.Equal(7.65m, receipt.TotalTaxes);
        Assert.Equal(65.15m, receipt.Total);
        var lines = receipt.Lines.Select(l => l.ToString()).ToList();
        Assert.Contains(lines, l => l.StartsWith("1 imported box of chocolates:"));
        Assert.Contains(lines, l => l.StartsWith("1 imported bottle of perfume:"));
    }

    [Fact]
    public void Basket3()
    {
        var receipt = Build(
            "1 imported bottle of perfume at 27.99",
            "1 bottle of perfume at 18.99",
            "1 packet of headache pills at 9.75",
            "1 imported box of chocolates at 11.25");
        Assert.Equal(6.70m, receipt.TotalTaxes);
        Assert.Equal(74.68m, receipt.Total);
        var lines = receipt.Lines.Select(l => l.ToString()).ToList();
        Assert.Contains(lines, l => l.StartsWith("1 imported bottle of perfume:"));
        Assert.Contains(lines, l => l.StartsWith("1 imported box of chocolates:"));
    }

}
