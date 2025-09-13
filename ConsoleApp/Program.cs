using SalesTaxCalculator.Core.Models;
using SalesTaxCalculator.Core.Parsing;
using SalesTaxCalculator.Core.Services;

static void PrintHeader(string title)
{
	Console.WriteLine(new string('-', title.Length));
	Console.WriteLine(title);
	Console.WriteLine(new string('-', title.Length));
}

IEnumerable<LineItem> Basket(params string[] lines) => lines.Select(l => InputLineParser.Parse(l));

var receiptBuilder = new ReceiptBuilder();

var baskets = new (string Title, IEnumerable<LineItem> Items)[]
{
	("Basket 1", Basket(
		"1 book at 12.49",
		"1 music CD at 14.99",
		"1 chocolate bar at 0.85")),
	("Basket 2", Basket(
		"1 imported box of chocolates at 10.00",
		"1 imported bottle of perfume at 47.50")),
	("Basket 3", Basket(
		"1 imported bottle of perfume at 27.99",
		"1 bottle of perfume at 18.99",
		"1 packet of headache pills at 9.75",
		"1 imported box of chocolates at 11.25"))
};

foreach (var (title, items) in baskets)
{
	PrintHeader(title);
	var receipt = receiptBuilder.Build(items);
	Console.WriteLine(receipt.ToString());
	Console.WriteLine();
}
