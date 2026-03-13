using Dapper;
using ExpenseTrackerCleanArch.Application.Features.Expenses;
using ExpenseTrackerCleanArch.Application.Interfaces;
using System.Data;

namespace ExpenseTrackerCleanArch.Infrastructure.Persistence;

public class DapperExpenseReadRepository : IExpenseReadRepository
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public DapperExpenseReadRepository(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<ExpenseDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();

        var sql = @"SELECT Id, Title, Amt, Category, Date FROM Expenses";

        var result = await connection.QueryAsync<ExpenseDto>(sql);

        return result;
    }

    public async Task<ExpenseDto?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();

        var sql = @"SELECT Id, Title, Amt, Category, Date 
                    FROM Expenses 
                    WHERE Id = @Id";

        var result = await connection.QueryFirstOrDefaultAsync<ExpenseDto>(
            sql,
            new { Id = id });

        return result;
    }
}