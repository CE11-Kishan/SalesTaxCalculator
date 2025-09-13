using SalesTaxCalculator.Core.Models;
namespace SalesTaxCalculator.Core.Tax;

public sealed class BasicSalesTaxRule : ITaxRule
{
    private readonly decimal _rate;
    private static readonly ProductCategory[] Exempt = { ProductCategory.Book, ProductCategory.Food, ProductCategory.Medical };

    public BasicSalesTaxRule(decimal rate = 0.10m)
    {
        _rate = rate;
    }

    public decimal Calculate(LineItem item)
    {
        if (Exempt.Contains(item.Product.Category)) return 0m;
        return item.NetPrice * _rate;
    }
}
