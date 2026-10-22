# MyBudget

A layered C# / .NET 10 console app for tracking monthly expenses against a budget. It keeps
one-time and recurring expenses, reports totals by category, warns when you are close to or
over your monthly limit, and saves everything to a JSON file between runs.

Built for the Advanced C# course at Humber Polytechnic. The course supplied the interfaces,
the console UI and the xUnit test suite; I designed and implemented the domain model,
services, persistence and dependency-injection wiring behind them.

## Architecture

```
MyBudget.sln
├─ MyBudget.Core/     domain model and business rules (no I/O)
│   ├─ Expenses.cs           Expense record hierarchy
│   ├─ ExpenseFactory.cs     validation and creation
│   ├─ ExpenseRepository.cs  in-memory collection + aggregate queries
│   └─ BudgetService.cs      monthly limit and budget status
├─ MyBudget.Data/     persistence
│   └─ JsonExpenseStore.cs   IExpenseStore backed by a JSON file
├─ MyBudget.App/      console UI + composition root (Program.cs)
└─ MyBudget.Tests/    xUnit tests with a hand-written in-memory fake
```

Core depends only on abstractions. The repository talks to `IExpenseStore`, not to a file,
so the same code runs against `JsonExpenseStore` in the app and `InMemoryExpenseStore` in tests.

## What I built

- **Expense model.** An abstract `Expense` record with `OneTimeExpense` and `RecurringExpense`
  subtypes. Each one computes its own `MonthlyImpact` (a recurring expense multiplies its amount
  by `TimesPerMonth`) and renders its own report line through `IReportable`. Polymorphic JSON
  attributes (`[JsonPolymorphic]` / `[JsonDerivedType]`) let the base type round-trip to disk
  without losing the subtype.
- **Validation in one place.** `ExpenseFactory` rejects amounts that are zero, negative or over
  1,000,000, rounds to two decimals, rejects recurring expenses that occur less than once a month,
  and assigns each expense a `Guid`.
- **Repository with LINQ queries.** `ExpenseRepository` loads from the injected store, then serves
  expenses ordered by date, the overall monthly total, totals grouped by category, and a
  per-category filter, and saves back through the store.
- **Budget rules.** `BudgetService` holds an encapsulated monthly limit and classifies spending as
  `NotSet`, `OnTrack`, `AlmostOut` (under 10% of the limit left) or `OverBudget`.
- **JSON persistence.** `JsonExpenseStore` returns an empty list when the file does not exist yet
  and writes the full list back on save.
- **Dependency injection.** `Program.cs` uses `Microsoft.Extensions.Hosting` to register the
  store, repository, budget service and UI as singletons against their interfaces, then resolves
  and runs the console app.

## Concepts practiced

Records and inheritance, abstract members and overrides, interfaces, encapsulation, custom
exceptions, LINQ (`OrderBy`, `GroupBy`, `ToDictionary`, `Sum`), `System.Text.Json` polymorphic
serialization, dependency inversion, the repository pattern, DI containers and service lifetimes,
and unit testing with fakes instead of a mocking framework.

## Run it

Requires the [.NET 10 SDK](https://dotnet.microsoft.com/download).

```bash
dotnet run --project MyBudget.App
```

Expenses are saved to `expenses.json` next to the built app.

## Test it

```bash
dotnet test
```
