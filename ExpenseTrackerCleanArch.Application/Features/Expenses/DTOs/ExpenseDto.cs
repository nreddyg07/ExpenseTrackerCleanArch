namespace ExpenseTrackerCleanArch.Application.Features.Expenses.DTOs;

public class ExpenseDto
{
	public int Id { get; set; }

	public string Title { get; set; } = string.Empty;

	public decimal Amt { get; set; }

	public string Category { get; set; } = string.Empty;

	public DateTime Date { get; set; }
}
