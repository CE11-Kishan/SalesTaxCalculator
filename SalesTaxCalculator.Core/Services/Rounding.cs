namespace SalesTaxCalculator.Core.Services;

public static class Rounding
{
    private const decimal Increment = 0.05m; // Smallest currency rounding step per requirements
    private const decimal Multiplier = 1m / Increment; // 20 when Increment is 0.05
    public static decimal RoundUpToNearestFiveCents(decimal amount)
    {
        if (amount == 0) return 0m;
        // Multiply by Multiplier (e.g., 20), ceiling, then divide back to apply increment rounding up.
        return Math.Ceiling(amount * Multiplier) / Multiplier;
    }
}
