
using System.Text.Json.Serialization;

namespace MyBudget.Core;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "kind")]
[JsonDerivedType(typeof(OneTimeExpense), "onetime")]
[JsonDerivedType(typeof(RecurringExpense), "recurring")]
public abstract record Expense(Guid Id, string Description,
        decimal Amount, ExpenseCategory Category, DateOnly Date) : IReportable
{
    public abstract decimal MonthlyImpact { get; } //— the cost within one month.

    
    public virtual string ToReportLine()    //— a one-line summary of the expense
    {
        return $"{Date: yyyy-MM-dd}  {Category} {Description} ${MonthlyImpact:0.00}";
    } 
}

public record OneTimeExpense(Guid Id, string Description, decimal Amount, ExpenseCategory Category, DateOnly Date)
    : Expense (Id, Description, Amount, Category, Date)
{
    //•	RecurringExpense (the five members plus int TimesPerMonth): 
    public override decimal MonthlyImpact => Amount;
}

public record RecurringExpense (Guid Id, string Description, decimal Amount, ExpenseCategory Category, DateOnly Date, int TimesPerMonth): Expense (Id, Description, Amount, Category, Date)
{
    //•	MonthlyImpact => Amount * TimesPerMonth, and override ToReportLine() to append "(x{TimesPerMonth}/month)".
    public override decimal MonthlyImpact => Amount * TimesPerMonth;
    public override string ToReportLine()
    {
        return base.ToReportLine() + $"(x{TimesPerMonth}/month)";
    }
}