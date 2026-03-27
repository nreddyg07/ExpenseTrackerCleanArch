namespace ExpenseTrackerCleanArch.Domain.Entities;

public class Expense
{
    public int Id { get; set; }
    //public string Title { get; set; }
    private string _title = string.Empty;
    public string Title
    {
        get => _title;
        set => _title = value?.Trim() ?? string.Empty;
    }

    public decimal Amt { get; set; }

    //public string Category { get; set; }= string.Empty;
    private string _cat = string.Empty;
    public string Category
    {
        get => _cat;
        set => _cat=value?.Trim() ?? string.Empty; } 

    public DateTime Date { get; set; }
}