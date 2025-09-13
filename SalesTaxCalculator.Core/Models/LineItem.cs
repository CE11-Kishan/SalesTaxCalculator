namespace SalesTaxCalculator.Core.Models;

public sealed record LineItem(Product Product, int Quantity)
{
    public decimal NetPrice => Product.UnitPrice * Quantity;
}
