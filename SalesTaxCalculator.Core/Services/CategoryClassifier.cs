using SalesTaxCalculator.Core.Enums;

namespace SalesTaxCalculator.Core.Services;

public static class CategoryClassifier
{
    public static (ProductCategory category, bool imported) Classify(string rawName)
    {
        bool imported = rawName.Contains("imported", StringComparison.OrdinalIgnoreCase);
        string lower = rawName.ToLowerInvariant();
        ProductCategory category = ProductCategory.Other;
        if (lower.Contains("book")) category = ProductCategory.Book;
        else if (lower.Contains("chocolate") || lower.Contains("chocolates")) category = ProductCategory.Food;
        else if (lower.Contains("pill")) category = ProductCategory.Medical;
        return (category, imported);
    }
}
