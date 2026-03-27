using ExpenseTrackerCleanArch.Application.Features.Expenses;
using ExpenseTrackerCleanArch.Domain.Entities;
using System.Threading;

namespace ExpenseTrackerCleanArch.Application.Interfaces;

public interface IExpenseWriteRepository
{
    Task<int> AddAsync(
        string title,
        decimal amt,
        string category,
        DateTime date,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        int id,
        string title,
        decimal amt,
        string category,
        DateTime date,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        int id,
        CancellationToken cancellationToken);

    Task<int> DeleteMultipleAsync(List<int> ids, CancellationToken cancellationToken);
    //Task UpdateAsync(ExpenseDto existing, CancellationToken cancellationToken);
    //Task AddAsync(Expense expense, CancellationToken cancellationToken);
}