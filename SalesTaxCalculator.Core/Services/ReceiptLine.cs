using SalesTaxCalculator.Core.Models;
namespace SalesTaxCalculator.Core.Services;

public sealed record ReceiptLine(LineItem Item, decimal Tax, decimal Gross)
{
    public override string ToString() => $"{Item.Quantity} {Item.Product.Name}: {Gross:0.00}";
}
