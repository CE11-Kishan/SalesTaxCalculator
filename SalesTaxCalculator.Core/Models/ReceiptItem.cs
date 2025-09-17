namespace SalesTaxCalculator.Core.Models;

public sealed record ReceiptItem(BasketItem Item, decimal Tax, decimal Gross)
{
    public override string ToString() => $"{Item.Quantity} {Item.Product.Name}: {Gross:0.00}";
}