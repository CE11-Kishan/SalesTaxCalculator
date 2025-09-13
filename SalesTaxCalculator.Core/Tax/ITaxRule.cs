using SalesTaxCalculator.Core.Models;
namespace SalesTaxCalculator.Core.Tax;

public interface ITaxRule
{
    decimal Calculate(LineItem item);
}
