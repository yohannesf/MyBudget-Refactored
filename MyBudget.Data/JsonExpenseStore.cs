/*
•	JsonExpenseStore(string path) — remember the file path.
•	Load() — return an empty list when the file is missing or empty; otherwise read and deserialize a List<Expense>.
•	Save(IEnumerable<Expense> expenses) — serialize to JSON (indented) and write the file.
*/

using MyBudget.Core;
using System.Text.Json;

namespace MyBudget.Data;

public class JsonExpenseStore : IExpenseStore
{
    public string _path; //

    public JsonExpenseStore(string path)
    {
        _path = path;   //JsonExpenseStore(string path) — remember the file path.
    }

    public IReadOnlyList<Expense> Load()
    {
        if (!File.Exists(_path)) return new List<Expense>();   //return an empty list when the file is missing or empty;

        string readJson = File.ReadAllText(_path);

        return JsonSerializer.Deserialize<List<Expense>>(readJson) ?? new List<Expense>();   //otherwise read and deserialize a List<Expense>

    }

    public void Save (IEnumerable<Expense> expenses)
    {
        string readJson = JsonSerializer.Serialize(expenses);
        File.WriteAllText(_path, readJson);
    }
}