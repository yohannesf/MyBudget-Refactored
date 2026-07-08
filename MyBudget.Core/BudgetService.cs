namespace MyBudget.Core;

/*
•	decimal MonthlyLimit { get; private set; } — encapsulated state.
•	SetMonthlyLimit(decimal limit): reject ≤ 0; store rounded to 2 decimals.
•	Remaining(decimal totalSpent) => MonthlyLimit - totalSpent.
•	Evaluate(decimal totalSpent): return NotSet when no limit; OverBudget when remaining < 0; AlmostOut when less than 10% of the limit remains; otherwise OnTrack. 

*/
public class BudgetService : IBudgetService
{
    public decimal MonthlyLimit {get; private set;}

    public void SetMonthlyLimit(decimal limit)
    {
        if (limit <= 0 ) throw new InvalidExpenseException($"Monthly limit must be greater than 0");
        MonthlyLimit = Math.Round(limit, 2);
    }

    public decimal Remaining(decimal totalSpent) => MonthlyLimit - totalSpent;

    public BudgetStatus Evaluate(decimal totalSpent)
    {
        if (MonthlyLimit == 0)
        return BudgetStatus.NotSet;

        decimal remaining = Remaining(totalSpent);

        if (remaining < 0m)
            return BudgetStatus.OverBudget;
        if (remaining < MonthlyLimit * 0.10m)
            return BudgetStatus.AlmostOut;
        return BudgetStatus.OnTrack;


    }
}