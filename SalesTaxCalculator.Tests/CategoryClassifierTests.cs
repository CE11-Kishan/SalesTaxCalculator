namespace SalesTaxCalculator.Tests;

using SalesTaxCalculator.Core.Enums;
using SalesTaxCalculator.Core.Services;
using Xunit;

public class CategoryClassifierTests
{
    [Theory]
    [InlineData("book", ProductCategory.Book, false)]
    [InlineData("imported book", ProductCategory.Book, true)]
    [InlineData("music CD", ProductCategory.Other, false)]
    [InlineData("imported music CD", ProductCategory.Other, true)]
    [InlineData("chocolate bar", ProductCategory.Food, false)]
    [InlineData("box of chocolates", ProductCategory.Food, false)]
    [InlineData("imported box of chocolates", ProductCategory.Food, true)]
    [InlineData("packet of headache pills", ProductCategory.Medical, false)]
    [InlineData("imported pills", ProductCategory.Medical, true)]
    [InlineData("bottle of perfume", ProductCategory.Other, false)]
    [InlineData("imported bottle of perfume", ProductCategory.Other, true)]
    public void Classify_ReturnsCorrectCategoryAndImported(string productName, ProductCategory expectedCategory, bool expectedImported)
    {
        var (category, imported) = CategoryClassifier.Classify(productName);
        
        Assert.Equal(expectedCategory, category);
        Assert.Equal(expectedImported, imported);
    }
    
    [Fact]
    public void Classify_ImportedCaseInsensitive()
    {
        var (_, imported1) = CategoryClassifier.Classify("IMPORTED book");
        var (_, imported2) = CategoryClassifier.Classify("Imported book");
        var (_, imported3) = CategoryClassifier.Classify("imported BOOK");
        
        Assert.True(imported1);
        Assert.True(imported2);
        Assert.True(imported3);
    }
}
