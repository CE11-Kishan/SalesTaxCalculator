namespace SalesTaxCalculator.Core.Services;

public sealed class Receipt
{
    public IReadOnlyList<ReceiptLine> Lines { get; }
    public decimal TotalTaxes { get; }
    public decimal Total { get; }

    public Receipt(IEnumerable<ReceiptLine> lines)
    {
        Lines = lines.ToList();
        TotalTaxes = Lines.Sum(l => l.Tax);
        Total = Lines.Sum(l => l.Gross);
    }

    public override string ToString()
    {
        var sb = new System.Text.StringBuilder();
        foreach (var line in Lines)
        {
            sb.AppendLine(line.ToString());
        }
        sb.AppendLine($"Sales Taxes: {TotalTaxes:0.00}");
        sb.Append($"Total: {Total:0.00}");
        return sb.ToString();
    }
}
