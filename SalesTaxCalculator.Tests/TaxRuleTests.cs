namespace SalesTaxCalculator.Tests;

using SalesTaxCalculator.Core.Enums;
using SalesTaxCalculator.Core.Models;
using SalesTaxCalculator.Core.Tax;
using Xunit;

public class TaxRuleTests
{
    private static BasketItem CreateBasketItem(string name, decimal price, ProductCategory category = ProductCategory.Other, bool imported = false, int quantity = 1)
    {
        var product = new Product(name, price, category, imported);
        return new BasketItem(product, quantity);
    }

    [Theory]
    [InlineData(ProductCategory.Book, 0.0)]
    [InlineData(ProductCategory.Food, 0.0)]
    [InlineData(ProductCategory.Medical, 0.0)]
    [InlineData(ProductCategory.Other, 1.499)]
    public void BasicSalesTaxRule_AppliesCorrectly(ProductCategory category, decimal expectedTax)
    {
        var rule = new BasicSalesTaxRule();
        var item = CreateBasketItem("test item", 14.99m, category);

        var result = rule.Calculate(item);

        Assert.Equal(expectedTax, result);
    }

    [Theory]
    [InlineData(false, 0.0)]
    [InlineData(true, 0.75)]
    public void ImportDutyTaxRule_AppliesCorrectly(bool imported, decimal expectedTax)
    {
        var rule = new ImportDutyTaxRule();
        var item = CreateBasketItem("test item", 15.00m, ProductCategory.Other, imported);

        var result = rule.Calculate(item);

        Assert.Equal(expectedTax, result);
    }

    [Fact]
    public void BasicSalesTaxRule_WithCustomRate()
    {
        var rule = new BasicSalesTaxRule(0.15m);
        var item = CreateBasketItem("test item", 10.00m, ProductCategory.Other);

        var result = rule.Calculate(item);

        Assert.Equal(1.50m, result);
    }

    [Fact]
    public void ImportDutyTaxRule_WithCustomRate()
    {
        var rule = new ImportDutyTaxRule(0.08m);
        var item = CreateBasketItem("test item", 25.00m, ProductCategory.Other, imported: true);

        var result = rule.Calculate(item);

        Assert.Equal(2.00m, result);
    }

    [Fact]
    public void BasicSalesTaxRule_ImportedExemptItem_NoTax()
    {
        var rule = new BasicSalesTaxRule();
        var item = CreateBasketItem("imported book", 12.49m, ProductCategory.Book, imported: true);

        var result = rule.Calculate(item);

        Assert.Equal(0m, result);
    }

    [Fact]
    public void ImportDutyTaxRule_ExemptItem_StillApplies()
    {
        var rule = new ImportDutyTaxRule();
        var item = CreateBasketItem("imported chocolate", 10.00m, ProductCategory.Food, imported: true);

        var result = rule.Calculate(item);

        Assert.Equal(0.50m, result); // Import duty has no exemptions
    }
}