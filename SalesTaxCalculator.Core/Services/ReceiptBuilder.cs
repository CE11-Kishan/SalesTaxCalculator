using SalesTaxCalculator.Core.Models;
namespace SalesTaxCalculator.Core.Services;

public sealed class ReceiptBuilder
{
    private readonly TaxCalculator _taxCalculator;

    public ReceiptBuilder(TaxCalculator? taxCalculator = null)
    {
        _taxCalculator = taxCalculator ?? new TaxCalculator();
    }

    public Receipt Build(IEnumerable<LineItem> items)
    {
        var lines = new List<ReceiptLine>();
        foreach (var item in items)
        {
            var tax = _taxCalculator.CalculateTotalTax(item);
            var gross = item.NetPrice + tax;
            lines.Add(new ReceiptLine(item, tax, gross));
        }
        return new Receipt(lines);
    }
}
