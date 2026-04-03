using Dapper;

namespace ExpenseTrackerCleanArch.Infrastructure.Persistence.QueryHelpers;

public class QueryHelper : IQueryHelper
{
    private readonly IQueryContext _context;

    public QueryHelper(IQueryContext context)
    {
        _context = context;
    }

    private string LoadSql(string fileName)
    {
        var path = Path.Combine(
            AppContext.BaseDirectory,
            "Persistence",
            "QueryHelpers",
            "SqlQueries",
            fileName
        );

        return File.ReadAllText(path);
    }

    public async Task<IEnumerable<T>> QueryAsync<T>(string fileName, object? param = null)
    {
        var sql = LoadSql(fileName);

        using var conn = _context.CreateConnection();
        return await conn.QueryAsync<T>(sql, param);
    }

    public async Task<T?> QueryFirstAsync<T>(string fileName, object? param = null)
    {
        var sql = LoadSql(fileName);

        using var conn = _context.CreateConnection();
        return await conn.QueryFirstOrDefaultAsync<T>(sql, param);
    }
}