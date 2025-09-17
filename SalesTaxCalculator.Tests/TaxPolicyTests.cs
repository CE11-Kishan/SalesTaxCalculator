namespace SalesTaxCalculator.Tests;

using SalesTaxCalculator.Core.Enums;
using SalesTaxCalculator.Core.Models;
using SalesTaxCalculator.Core.Tax;
using Xunit;

public class TaxPolicyTests
{
    private static BasketItem CreateBasketItem(string name, decimal price, ProductCategory category = ProductCategory.Other, bool imported = false, int quantity = 1)
    {
        var product = new Product(name, price, category, imported);
        return new BasketItem(product, quantity);
    }

    [Fact]
    public void Price_ExemptBook_NoTax()
    {
        var policy = new TaxPolicy();
        var rounding = new RoundingPolicy();
        var item = CreateBasketItem("book", 12.49m, ProductCategory.Book);

        var (rounded, raw, portions) = policy.Price(item, rounding);

        Assert.Equal(0m, rounded);
        Assert.Equal(0m, raw);
        Assert.Empty(portions);
    }

    [Fact]
    public void Price_ExemptFood_NoTax()
    {
        var policy = new TaxPolicy();
        var rounding = new RoundingPolicy();
        var item = CreateBasketItem("chocolate bar", 0.85m, ProductCategory.Food);

        var (rounded, raw, portions) = policy.Price(item, rounding);

        Assert.Equal(0m, rounded);
        Assert.Equal(0m, raw);
        Assert.Empty(portions);
    }

    [Fact]
    public void Price_ExemptMedical_NoTax()
    {
        var policy = new TaxPolicy();
        var rounding = new RoundingPolicy();
        var item = CreateBasketItem("headache pills", 9.75m, ProductCategory.Medical);

        var (rounded, raw, portions) = policy.Price(item, rounding);

        Assert.Equal(0m, rounded);
        Assert.Equal(0m, raw);
        Assert.Empty(portions);
    }

    [Fact]
    public void Price_TaxableItem_BasicTaxApplied()
    {
        var policy = new TaxPolicy();
        var rounding = new RoundingPolicy();
        var item = CreateBasketItem("music CD", 14.99m, ProductCategory.Other);

        var (rounded, raw, portions) = policy.Price(item, rounding);

        Assert.Equal(1.50m, rounded); // 14.99 * 0.10 = 1.499 -> rounds up to 1.50
        Assert.Equal(1.499m, raw);
        Assert.Single(portions);
        Assert.Equal("BasicSalesTax", portions[0].Name);
        Assert.Equal(0.10m, portions[0].Rate);
        Assert.Equal(1.499m, portions[0].Amount);
    }

    [Fact]
    public void Price_ImportedExemptItem_ImportDutyOnly()
    {
        var policy = new TaxPolicy();
        var rounding = new RoundingPolicy();
        var item = CreateBasketItem("imported box of chocolates", 10.00m, ProductCategory.Food, imported: true);

        var (rounded, raw, portions) = policy.Price(item, rounding);

        Assert.Equal(0.50m, rounded); // 10.00 * 0.05 = 0.50 (no rounding needed)
        Assert.Equal(0.50m, raw);
        Assert.Single(portions);
        Assert.Equal("ImportDutyTax", portions[0].Name);
        Assert.Equal(0.05m, portions[0].Rate);
        Assert.Equal(0.50m, portions[0].Amount);
    }

    [Fact]
    public void Price_ImportedTaxableItem_BothTaxes()
    {
        var policy = new TaxPolicy();
        var rounding = new RoundingPolicy();
        var item = CreateBasketItem("imported bottle of perfume", 47.50m, ProductCategory.Other, imported: true);

        var (rounded, raw, portions) = policy.Price(item, rounding);

        // Basic: 47.50 * 0.10 = 4.75
        // Import: 47.50 * 0.05 = 2.375
        // Raw total: 7.125 -> rounds up to 7.15
        Assert.Equal(7.15m, rounded);
        Assert.Equal(7.125m, raw);
        Assert.Equal(2, portions.Count);
        
        var basicPortion = portions.First(p => p.Name == "BasicSalesTax");
        var importPortion = portions.First(p => p.Name == "ImportDutyTax");
        
        Assert.Equal(0.10m, basicPortion.Rate);
        Assert.Equal(4.75m, basicPortion.Amount);
        Assert.Equal(0.05m, importPortion.Rate);
        Assert.Equal(2.375m, importPortion.Amount);
    }

    [Fact]
    public void Price_QuantityMultiplier_AffectsTax()
    {
        var policy = new TaxPolicy();
        var rounding = new RoundingPolicy();
        var item = CreateBasketItem("music CD", 14.99m, ProductCategory.Other, quantity: 2);

        var (rounded, raw, portions) = policy.Price(item, rounding);

        // Net price: 14.99 * 2 = 29.98
        // Tax: 29.98 * 0.10 = 2.998 -> rounds up to 3.00
        Assert.Equal(3.00m, rounded);
        Assert.Equal(2.998m, raw);
    }
}
