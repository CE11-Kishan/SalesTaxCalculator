using SalesTaxCalculator.Core.Tax;

namespace SalesTaxCalculator.Core.Models;

public sealed class Basket
{
    private readonly List<BasketItem> _items = new();
    public IReadOnlyList<BasketItem> Items => _items;

    public void Add(BasketItem item)
    {
        if (item is null) throw new ArgumentNullException(nameof(item));
        if (item.Quantity <= 0) throw new ArgumentException("Quantity must be positive", nameof(item));
        _items.Add(item);
    }

    public Receipt GenerateReceipt(TaxPolicy? taxPolicy = null, RoundingPolicy? rounding = null)
    {
        var policy = taxPolicy ?? new TaxPolicy();
        var roundingPolicy = rounding ?? new RoundingPolicy();
        
        var receiptItems = new List<ReceiptItem>();
        foreach (var item in _items)
        {
            var (roundedTax, _, _) = policy.Price(item, roundingPolicy);
            var grossPrice = item.NetPrice + roundedTax;
            receiptItems.Add(new ReceiptItem(item, roundedTax, grossPrice));
        }
        
        return new Receipt(receiptItems);
    }
}
