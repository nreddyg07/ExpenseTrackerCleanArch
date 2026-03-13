namespace ExpenseTrackerCleanArch.Domain.Entities;

public class Expense
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public decimal Amt { get; set; }

    public string Category { get; set; } = string.Empty;

    public DateTime Date { get; set; }
}