using SalesTaxCalculator.Core.Models;

namespace SalesTaxCalculator.Core.Tax;

public sealed class TaxPolicy
{
    private readonly IReadOnlyList<ITaxRule> _rules;

    public TaxPolicy(IEnumerable<ITaxRule>? rules = null)
    {
        _rules = (rules ?? new ITaxRule[] { new BasicSalesTaxRule(), new ImportDutyTaxRule() }).ToList();
    }

    public (decimal RoundedTax, decimal RawTax, IReadOnlyList<TaxPortion> Portions) Price(BasketItem item, RoundingPolicy rounding)
    {
        var portions = new List<TaxPortion>(_rules.Count);
        foreach (var rule in _rules)
        {
            var before = rule.Calculate(item);
            if (before > 0m)
            {
                var name = rule.GetType().Name.Replace("Rule", string.Empty);
                decimal rate = before / item.NetPrice;
                portions.Add(new TaxPortion(name, rate, before));
            }
        }
        var raw = portions.Sum(p => p.Amount);
        var rounded = rounding.Round(raw);
        return (rounded, raw, portions);
    }
}
