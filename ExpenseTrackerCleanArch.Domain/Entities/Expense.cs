namespace ExpenseTrackerCleanArch.Domain.Entities;

public class Expense
{
    public int Id { get; private set; }

    public string Title { get; private set; } = string.Empty;

    public decimal Amt { get; private set; }

    public string? Category { get; private set; }

    public DateTime Date { get; private set; }

    private Expense() { } // Required by EF

    public Expense(string title, decimal amt, string? category, DateTime date)
    {
        if (amt <= 0)
            throw new ArgumentException("Amount must be greater than zero");

        Title = title;
        Amt = amt;
        Category = category;
        Date = date;
    }

    public void Update(string title, decimal amt, string? category, DateTime date)
    {
        if (amt <= 0)
            throw new ArgumentException("Amount must be greater than zero");

        Title = title;
        Amt = amt;
        Category = category;
        Date = date;
    }
}