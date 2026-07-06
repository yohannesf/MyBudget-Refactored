using System.Security.Cryptography.X509Certificates;

namespace MyBudget.Core;

public static class ExpenseFactory
{
    public static decimal ValidateAmount(decimal amount)
    {
        if (amount <=0 || amount > 1_000_000m)
        {
            throw new InvalidExpenseException ($"Invalid amount must be positive and below 1M");
        }
        return Math.Round(amount,2);
    }

    public static OneTimeExpense  CreateOneTime(string description, decimal amount,
                                  ExpenseCategory category, DateOnly date)
    {
        return new OneTimeExpense(Guid.NewGuid(), description, ValidateAmount(amount), category, date);
    }
    

    public static RecurringExpense CreateRecurring(string description, decimal amount,
                                  ExpenseCategory category, DateOnly date, int timesPerMonth)
    {
        return new RecurringExpense(Guid.NewGuid(), description, ValidateAmount(amount), category, date, timesPerMonth);
    }

}