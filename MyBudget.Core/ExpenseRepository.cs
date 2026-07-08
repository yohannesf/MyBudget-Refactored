/*
•	Holds expenses in a List<Expense>: A private backing field
•	Depends on IExpenseStore, injected through its constructor. 
            ExpenseRepository(IExpenseStore store) — load existing expenses from the store into the list.
•	GetAll() — all expenses ordered by date.
•	Add(Expense expense) — append (reject null).
•	Total() — sum of every expense's MonthlyImpact.
•	TotalsByCategory() — group by category and sum into a dictionary.
•	InCategory(ExpenseCategory category) — filter to one category, ordered by date.
•	Save() — persist the current list through the store.
*/

namespace MyBudget.Core;

public class ExpenseRepository: IExpenseRepository
{
    private  List<Expense> _expenses;
    private IExpenseStore _store;

    public ExpenseRepository(IExpenseStore store)
    {
        _store = store;
        _expenses = store.Load().ToList();  //load existing expenses from the store into the list
    }

    public IReadOnlyList<Expense> GetAll()  //from IExpenseRepostiory interface
    {
        return _expenses.OrderBy(e=> e.Date).ToList();
    }

    public void Add(Expense expense)    //from IExpenseRepostiory interface
    {
        if (expense is null) throw new InvalidExpenseException("Expnse cannot be null");
        _expenses.Add(expense); //append the expense to the list
    }

    public decimal Total()
    {
        return _expenses.Sum(e=> e.MonthlyImpact);    //Total() — sum of every expense's MonthlyImpact
    }

    public IReadOnlyDictionary<ExpenseCategory, decimal> TotalsByCategory()
    {
        //TotalsByCategory() — group by category and sum into a dictionary.
        return _expenses.GroupBy(e=>e.Category).ToDictionary(c => c.Key, c=>c.Sum(e=> e.MonthlyImpact));
    }

    public IReadOnlyList<Expense> InCategory(ExpenseCategory category)
    {
        //InCategory(ExpenseCategory category) — filter to one category, ordered by date.
        return _expenses.Where(e=>e.Category == category)
                        .OrderBy( e => e.Date).ToList();
    }

    public void Save()
    {
        _store.Save(_expenses);  //Save() — persist the current list through the store.
    }



}