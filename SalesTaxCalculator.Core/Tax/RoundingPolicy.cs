using SalesTaxCalculator.Core.Services;

namespace SalesTaxCalculator.Core.Tax;

public sealed class RoundingPolicy
{
    public decimal Round(decimal rawTax) => Rounding.RoundUpToNearestFiveCents(rawTax);
}
