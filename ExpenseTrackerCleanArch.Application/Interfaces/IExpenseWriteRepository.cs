using ExpenseTrackerCleanArch.Application.Features.Expenses.DTOs;

namespace ExpenseTrackerCleanArch.Application.Interfaces;

public interface IExpenseWriteRepository
{
    Task<int> AddAsync(string title, decimal amt, string category, DateTime date, CancellationToken ct);

    Task<int> UpdateAsync(int id, string title, decimal amt, string category, DateTime date, CancellationToken ct);

    Task<int> DeleteAsync(int id, CancellationToken ct);

    Task<int> DeleteMultipleAsync(List<int> ids, CancellationToken ct);

    // Keeping these as DTOs since they are for bulk operations
    Task<int> AddMultipleAsync(IEnumerable<ExpenseDto> expenses, CancellationToken ct);

    Task<int> UpdateMultipleAsync(IEnumerable<ExpenseDto> expenses, CancellationToken ct);
}