using SalesTaxCalculator.Core.Models;using SalesTaxCalculator.Core.Tax;

namespace SalesTaxCalculator.Core.Services;

public sealed class TaxCalculator
{
    private readonly IReadOnlyCollection<ITaxRule> _rules;

    public TaxCalculator(IEnumerable<ITaxRule>? rules = null)
    {
        _rules = (rules ?? new ITaxRule[] { new BasicSalesTaxRule(), new ImportDutyTaxRule() }).ToArray();
    }

    public decimal CalculateTotalTax(LineItem item)
    {
        var raw = _rules.Sum(r => r.Calculate(item));
        return Rounding.RoundUpToNearestFiveCents(raw);
    }
}
