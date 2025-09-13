namespace SalesTaxCalculator.Core.Services;

public static class Rounding
{
    public static decimal RoundUpToNearestFiveCents(decimal amount)
    {
        if (amount == 0) return 0m;
        // Multiply by 20 (since 1/0.05 = 20), ceiling, then divide back
        return Math.Ceiling(amount * 20m) / 20m;
    }
}
