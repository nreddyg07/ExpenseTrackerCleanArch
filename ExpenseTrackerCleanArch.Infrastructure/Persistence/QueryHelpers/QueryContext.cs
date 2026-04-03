using System.Data;
using ExpenseTrackerCleanArch.Application.Interfaces;

namespace ExpenseTrackerCleanArch.Infrastructure.Persistence.QueryHelpers;

public class QueryContext : IQueryContext
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public QueryContext(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public IDbConnection CreateConnection()
    {
        return _connectionFactory.CreateConnection();
    }
}