using System.Data;

namespace ExpenseTrackerCleanArch.Infrastructure.Persistence.QueryHelpers;

public interface IQueryContext
{
    IDbConnection CreateConnection();
}