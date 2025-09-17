namespace SalesTaxCalculator.Core.Models;

public sealed class Receipt
{
    public IReadOnlyList<ReceiptItem> Lines { get; }
    public decimal TotalTaxes { get; }
    public decimal Total { get; }

    public Receipt(IEnumerable<ReceiptItem> lines)
    {
        Lines = lines.ToList();
        TotalTaxes = Lines.Sum(l => l.Tax);
        Total = Lines.Sum(l => l.Gross);
    }

    public override string ToString()
    {
        var result = string.Join(Environment.NewLine, Lines);
        result += Environment.NewLine + $"Sales Taxes: {TotalTaxes:0.00}";
        result += Environment.NewLine + $"Total: {Total:0.00}";
        return result;
    }
}