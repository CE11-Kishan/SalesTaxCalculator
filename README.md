# Sales Tax Calculator

Solution: `SalesTaxCalculator.sln`

Projects:
- `SalesTaxCalculator.Core` (domain + tax + services + parsing)
- `ConsoleApp` (entry point / presentation)
- `SalesTaxCalculator.Tests` (xUnit test suite)

## Features
- Domain model: Product, LineItem, Receipt
- Pluggable tax rules via `ITaxRule`
- Basic sales tax 10% (books/food/medical exempt)
- Import duty 5% (no exemptions)
- Combined per-line tax then rounded up to nearest 0.05
- Deterministic manual parser (`InputLineParser`) – no regex
- Comprehensive receipt formatting & totals
- xUnit tests covering the three sample baskets

## Quick Start
Run (from solution root):

```powershell
dotnet build
dotnet run --project .\ConsoleApp
```

Run tests:

```powershell
dotnet test
```

## Program Flow
ConsoleApp -> InputLineParser -> LineItem(s) -> ReceiptBuilder -> (Line: TaxCalculator => Rules => Rounding) -> Receipt -> Console Output

## Responsibilities Of Entities
- InputLineParser: parse text line into structured `LineItem` (qty, product, price)
- Product / LineItem: immutable value objects representing purchased goods
- ITaxRule & implementations: encapsulate independent tax policies
- TaxCalculator: aggregate rule results & apply rounding once per line
- ReceiptBuilder: transform items into receipt lines (net + tax + gross)
- ReceiptLine: presentable line value; holds tax and gross
- Receipt: collection aggregate computing totals and rendering
- Program (ConsoleApp): orchestrates sample baskets & printing

## Rounding Rule
Raw tax = sum(applicable rates * net price). Rounded tax = ceil(raw * 20) / 20. Gross = net + rounded tax.

## Design Principles & OOP Used In This Project
- Abstraction: `ITaxRule`, `InputLineParser`, `ReceiptBuilder` isolate behaviors.
- Encapsulation: `TaxCalculator` hides rule iteration + rounding; `Receipt` hides line aggregation.
- Polymorphism: Multiple `ITaxRule` implementations used uniformly.
- Composition over Inheritance: Objects (Receipt, LineItem, TaxCalculator) are built from smaller parts—no deep class hierarchy.
- Single Responsibility: Each class has one focused reason to change.
- Open/Closed: Add a tax by creating a new `ITaxRule` (no existing code edits).
- Interface Segregation: Tiny, purpose-fit interface `ITaxRule`.
- Dependency Inversion: High-level receipt logic depends on the `ITaxRule` abstraction list.
- Immutability: Records (`Product`, `LineItem`, `ReceiptLine`) avoid accidental state changes.