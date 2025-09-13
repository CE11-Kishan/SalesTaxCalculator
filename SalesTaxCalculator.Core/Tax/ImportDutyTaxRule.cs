using SalesTaxCalculator.Core.Models;
namespace SalesTaxCalculator.Core.Tax;

public sealed class ImportDutyTaxRule : ITaxRule
{
    private readonly decimal _rate;

    public ImportDutyTaxRule(decimal rate = 0.05m)
    {
        _rate = rate;
    }

    public decimal Calculate(LineItem item)
    {
        if (!item.Product.Imported) return 0m;
        return item.NetPrice * _rate;
    }
}
