using SalesTaxCalculator.Core.Models;
using SalesTaxCalculator.Core.Services;

static void PrintHeader(string title)
{
	Console.WriteLine(new string('-', title.Length));
	Console.WriteLine(title);
	Console.WriteLine(new string('-', title.Length));
}

static void RunDemo()
{
	IEnumerable<BasketItem> BasketItems(params string[] lines) => lines.Select(l => InputLineParser.Parse(l));

	var baskets = new (string Title, IEnumerable<BasketItem> Items)[]
	{
		("Basket 1", BasketItems(
			"1 book at 12.49",
			"1 music CD at 14.99",
			"1 chocolate bar at 0.85")),
		("Basket 2", BasketItems(
			"1 imported box of chocolates at 10.00",
			"1 imported bottle of perfume at 47.50")),
		("Basket 3", BasketItems(
			"1 imported bottle of perfume at 27.99",
			"1 bottle of perfume at 18.99",
			"1 packet of headache pills at 9.75",
			"1 imported box of chocolates at 11.25"))
	};

	Console.WriteLine("=== DEMO MODE ===");
	Console.WriteLine();

	foreach (var (title, items) in baskets)
	{
		PrintHeader(title);
		
		var basket = new Basket();
		foreach (var item in items)
			basket.Add(item);
			
		var receipt = basket.GenerateReceipt();
		Console.WriteLine(receipt.ToString());
		Console.WriteLine();
	}
}

static void RunInteractive()
{
	Console.WriteLine("=== INTERACTIVE MODE ===");
	Console.WriteLine("Enter items one per line (format: quantity product at price)");
	Console.WriteLine("Press Enter with empty line to finish and generate receipt");
	Console.WriteLine("Example: 1 book at 12.49");
	Console.WriteLine();

	var basket = new Basket();
	int lineNumber = 1;

	while (true)
	{
		Console.Write($"Item {lineNumber}: ");
		string? input = Console.ReadLine();
		
		if (string.IsNullOrWhiteSpace(input))
		{
			if (basket.Items.Count == 0)
			{
				Console.WriteLine("No items entered. Goodbye!");
				return;
			}
			break;
		}

		try
		{
			var item = InputLineParser.Parse(input);
			basket.Add(item);
			Console.WriteLine($"  ✓ Added: {item.Quantity} {item.Product.Name} at ${item.Product.UnitPrice:0.00}");
			lineNumber++;
		}
		catch (FormatException ex)
		{
			Console.WriteLine($"  ✗ Error: {ex.Message}");
			Console.WriteLine("  Please try again with format: quantity product at price");
		}
		catch (Exception ex)
		{
			Console.WriteLine($"  ✗ Unexpected error: {ex.Message}");
		}
	}

	Console.WriteLine();
	PrintHeader("Your Receipt");
	var receipt = basket.GenerateReceipt();
	Console.WriteLine(receipt.ToString());
}

if (args.Length > 0 && args[0].Equals("--demo", StringComparison.OrdinalIgnoreCase))
{
	RunDemo();
}
else
{
	RunInteractive();
}
