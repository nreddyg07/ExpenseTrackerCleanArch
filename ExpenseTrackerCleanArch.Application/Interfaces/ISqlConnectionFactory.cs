using System.Data;

namespace ExpenseTrackerCleanArch.Application.Interfaces;

public interface ISqlConnectionFactory
{
    IDbConnection CreateConnection();
}