using SalesTaxCalculator.Core.Models;
namespace SalesTaxCalculator.Core.Tax;

public sealed class ImportDutyTaxRule : ITaxRule
{
    private readonly decimal _rate;

    public ImportDutyTaxRule(decimal rate = TaxRates.ImportDuty)
    {
        _rate = rate;
    }

    public decimal Calculate(BasketItem item)
    {
        if (!item.Product.Imported) return 0m;
        return item.NetPrice * _rate;
    }
}
