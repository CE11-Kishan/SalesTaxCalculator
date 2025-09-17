# Sales Tax Calculator

A .NET console application for sales tax calculation.

## Solution Structure

Solution: `SalesTaxCalculator.sln`

Projects:
- `SalesTaxCalculator.Core` (domain models + tax engine + services)
- `ConsoleApp` (interactive console interface)
- `SalesTaxCalculator.Tests` (comprehensive xUnit test suite - 56 tests)

## Features

### Core Domain
- **Domain Models**: `Basket` aggregate, `Product`/`BasketItem` value objects, `Receipt` with behavior
- **Domain Services**: `TaxPolicy` for pricing logic, `RoundingPolicy` for business rules
- **Strategy Pattern**: Pluggable tax rules via `ITaxRule` interface
- **Value Objects**: `TaxPortion` for tax transparency and audit trails

### Tax Engine
- **Basic Sales Tax**: 10% (books/food/medical products exempt)
- **Import Duty**: 5% (no exemptions, applies to all imported goods)
- **Smart Rounding**: Up to nearest $0.05 using business rules

### User Experience
- **Interactive Mode**: User-friendly console input with validation and error handling
- **Demo Mode**: Use `--demo` flag to run predefined sample baskets

## Quick Start

### Run Interactive Mode (default)
```powershell
dotnet build
dotnet run --project .\ConsoleApp
```

### Run Demo Mode
```powershell
dotnet run --project .\ConsoleApp -- --demo
```

### Run Tests
```powershell
dotnet test
```

## Architecture Overview

### Object-Oriented Program Flow
```
Interactive Input → InputLineParser → BasketItem → Basket.Add() 
                                        ↓
Basket.GenerateReceipt() → TaxPolicy.Price() → ITaxRule[] → RoundingPolicy 
                                        ↓
                          ReceiptItem → Receipt → Console Output
```

### Domain Model Responsibilities

#### Core Aggregates
- **`Basket`**: Domain aggregate root with validation and receipt generation behavior
- **`Receipt`**: Encapsulates line items, calculates totals, handles formatting

#### Value Objects  
- **`Product`**: Immutable product information (name, price, category, import status)
- **`BasketItem`**: Shopping basket entry with calculated net price (`Product + Quantity`)
- **`ReceiptItem`**: Receipt line with tax and gross price calculations
- **`TaxPortion`**: Tax breakdown for transparency (`Name`, `Rate`, `Amount`)

#### Domain Services
- **`TaxPolicy`**: Orchestrates tax calculation using pluggable rules and rounding
- **`RoundingPolicy`**: Business rule wrapper around rounding logic

#### Strategy Pattern (Tax Rules)
- **`ITaxRule`**: Interface for extensible tax calculations
- **`BasicSalesTaxRule`**: 10% tax with exemptions for books/food/medical
- **`ImportDutyTaxRule`**: 5% import duty with no exemptions

#### Application Services
- **`InputLineParser`**: Pure syntax parsing (quantity, name, price extraction)
- **`CategoryClassifier`**: Semantic analysis (product categorization and import detection)
- **`Rounding`**: Mathematical rounding utilities with extracted constants

### Constants & Configuration
- **`TaxRates`**: Centralized tax rate constants (`Basic = 0.10m`, `ImportDuty = 0.05m`)
- **`ProductCategory`**: Enum for tax exemption categories (`Book`, `Food`, `Medical`, `Other`)

## Rounding Business Rule
```
Raw Tax = Sum(Tax Rule Amounts)
Rounded Tax = Ceiling(Raw Tax × 20) ÷ 20  // Up to nearest $0.05
Gross Price = Net Price + Rounded Tax
```

## Design Principles & OOP Used In This Project

- **Abstraction**: `ITaxRule`, `InputLineParser`, `CategoryClassifier` isolate behaviors
- **Encapsulation**: `TaxPolicy` hides rule iteration + rounding; `Receipt` hides line aggregation; `Basket` encapsulates item management
- **Polymorphism**: Multiple `ITaxRule` implementations used uniformly through common interface
- **Composition over Inheritance**: Objects built from smaller parts - no deep class hierarchy
- **Single Responsibility**: Each class has one focused reason to change
- **Open/Closed**: Add tax rules by creating new `ITaxRule` implementations (no existing code edits)
- **Interface Segregation**: Minimal, purpose-fit interface `ITaxRule`
- **Dependency Inversion**: High-level `TaxPolicy` depends on `ITaxRule` abstractions
- **Immutability**: Records (`Product`, `BasketItem`, `ReceiptItem`, `TaxPortion`) avoid accidental state changes

## Sample Output

### Interactive Mode
```
=== INTERACTIVE MODE ===
Enter items one per line (format: quantity product at price)
Press Enter with empty line to finish and generate receipt
Example: 1 book at 12.49

Item 1: 1 book at 12.49
  ✓ Added: 1 book at $12.49
Item 2: 1 music CD at 14.99
  ✓ Added: 1 music CD at $14.99
Item 3: 

------------
Your Receipt
------------
1 book: 12.49
1 music CD: 16.49
Sales Taxes: 1.50
Total: 28.98
```

### Demo Mode
```
=== DEMO MODE ===

--------
Basket 1
--------
1 book: 12.49
1 music CD: 16.49
1 chocolate bar: 0.85
Sales Taxes: 1.50
Total: 29.83
```

---
