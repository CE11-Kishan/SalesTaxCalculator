using SalesTaxCalculator.Core.Models;
namespace SalesTaxCalculator.Core.Parsing;

public static class InputLineParser
{
    public static LineItem Parse(string line)
    {
        if (string.IsNullOrWhiteSpace(line))
            throw new FormatException("Line is empty");

        line = line.Trim();
        int atIndex = line.LastIndexOf(" at ", StringComparison.Ordinal);
        if (atIndex <= 0)
            throw new FormatException($"Missing ' at ' delimiter in line: '{line}'");

        string left = line.Substring(0, atIndex);
        string pricePart = line.Substring(atIndex + 4);
        if (!decimal.TryParse(pricePart, out var price) || price < 0)
            throw new FormatException($"Invalid price '{pricePart}' in line: '{line}'");

        int firstSpace = left.IndexOf(' ');
        if (firstSpace <= 0)
            throw new FormatException($"Cannot find space after quantity in line: '{line}'");

        string qtyPart = left[..firstSpace];
        if (!int.TryParse(qtyPart, out var qty) || qty <= 0)
            throw new FormatException($"Invalid quantity '{qtyPart}' in line: '{line}'");

        string name = left[(firstSpace + 1)..].Trim();
        if (name.Length == 0)
            throw new FormatException($"Missing product name in line: '{line}'");

        var (category, imported) = Classify(name);
        var normalizedName = NormalizeSpaces(name);
        var product = new Product(normalizedName, price, category, imported);
        return new LineItem(product, qty);
    }

    private static string NormalizeSpaces(string input)
    {
        if (input.IndexOf(' ') < 0) return input;
        Span<char> buffer = stackalloc char[input.Length];
        int w = 0;
        bool prevSpace = false;
        foreach (var c in input)
        {
            if (char.IsWhiteSpace(c))
            {
                if (prevSpace) continue;
                buffer[w++] = ' ';
                prevSpace = true;
            }
            else
            {
                buffer[w++] = c;
                prevSpace = false;
            }
        }
        if (w > 0 && buffer[w - 1] == ' ') w--;
        return new string(buffer.Slice(0, w));
    }

    private static (ProductCategory category, bool imported) Classify(string rawName)
    {
        bool imported = rawName.Contains("imported", StringComparison.OrdinalIgnoreCase);
        string lower = rawName.ToLowerInvariant();
        ProductCategory category = ProductCategory.Other;
        if (lower.Contains("book")) category = ProductCategory.Book;
        else if (lower.Contains("chocolate") || lower.Contains("chocolates")) category = ProductCategory.Food;
        else if (lower.Contains("pill")) category = ProductCategory.Medical;
        return (category, imported);
    }
}
