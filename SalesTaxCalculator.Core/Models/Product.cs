namespace SalesTaxCalculator.Core.Models;

public sealed record Product(string Name, decimal UnitPrice, ProductCategory Category, bool Imported)
{
    public override string ToString() => Name;
}
