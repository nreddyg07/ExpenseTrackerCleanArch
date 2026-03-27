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
}