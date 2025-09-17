using SalesTaxCalculator.Core.Models;
using SalesTaxCalculator.Core.Enums;
namespace SalesTaxCalculator.Core.Tax;

public sealed class BasicSalesTaxRule : ITaxRule
{
    private readonly decimal _rate;
    private static readonly ProductCategory[] Exempt = { ProductCategory.Book, ProductCategory.Food, ProductCategory.Medical };

    public BasicSalesTaxRule(decimal rate = TaxRates.Basic)
    {
        _rate = rate;
    }

    public decimal Calculate(BasketItem item)
    {
        if (Exempt.Contains(item.Product.Category)) return 0m;
        return item.NetPrice * _rate;
    }
}
