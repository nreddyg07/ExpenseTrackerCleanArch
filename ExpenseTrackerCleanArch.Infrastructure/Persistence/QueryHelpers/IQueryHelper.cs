namespace ExpenseTrackerCleanArch.Infrastructure.Persistence.QueryHelpers;

public interface IQueryHelper
{
    Task<IEnumerable<T>> QueryAsync<T>(string fileName, object? param = null);
    Task<T?> QueryFirstAsync<T>(string fileName, object? param = null);
}