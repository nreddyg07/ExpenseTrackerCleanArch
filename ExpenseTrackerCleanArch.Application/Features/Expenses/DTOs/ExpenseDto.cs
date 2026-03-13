using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ExpenseTrackerCleanArch.Application.Features.Expenses.DTOs;

public class ExpenseDto
{
    //public int Id { get; set; }
    //public string Title { get; set; } = string.Empty;
    //public decimal Amt { get; set; }
    //public string? Category { get; set; }
    //public DateTime Date { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]  // Hide in Swagger during POST/PUT
    public int Id { get; set; }

    [Required]
    public string Title { get; set; } = null!;

    [Required]
    public decimal Amt { get; set; }

    public string? Category { get; set; }

    [Required]
    public DateTime Date { get; set; }
}
