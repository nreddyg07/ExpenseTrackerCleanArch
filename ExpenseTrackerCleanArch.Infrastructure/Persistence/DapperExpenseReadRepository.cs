using ExpenseTrackerCleanArch.Application.Features.Expenses;
using ExpenseTrackerCleanArch.Application.Interfaces;
using ExpenseTrackerCleanArch.Infrastructure.Persistence.QueryHelpers;

namespace ExpenseTrackerCleanArch.Infrastructure.Persistence;

public class DapperExpenseReadRepository : IExpenseReadRepository
{
    private readonly IQueryHelper _queryHelper;

    public DapperExpenseReadRepository(IQueryHelper queryHelper)
    {
        _queryHelper = queryHelper;
    }

    public async Task<IEnumerable<ExpenseDto>> GetAllAsync(CancellationToken ct)
    {
        return await _queryHelper.QueryAsync<ExpenseDto>("GetAllExpenses.sql");
    }

    public async Task<ExpenseDto?> GetByIdAsync(int id, CancellationToken ct)
    {
        return await _queryHelper.QueryFirstAsync<ExpenseDto>(
            "GetExpenseById.sql",
            new { Id = id }
        );
    }
}