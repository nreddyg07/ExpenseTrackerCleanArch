using ExpenseTrackerCleanArch.Application.Features.Expenses;

namespace ExpenseTrackerCleanArch.Application.Interfaces;

public interface IExpenseReadRepository
{
    Task<IEnumerable<ExpenseDto>> GetAllAsync(CancellationToken cancellationToken);

    Task<ExpenseDto?> GetByIdAsync(int id, CancellationToken cancellationToken);

}