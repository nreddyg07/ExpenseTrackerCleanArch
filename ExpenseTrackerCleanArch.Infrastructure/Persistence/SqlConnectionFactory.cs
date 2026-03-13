using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using ExpenseTrackerCleanArch.Application.Interfaces;

namespace ExpenseTrackerCleanArch.Infrastructure.Persistence;

public class SqlConnectionFactory : ISqlConnectionFactory
{
    private readonly IConfiguration _configuration;

    public SqlConnectionFactory(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IDbConnection CreateConnection()
    {
        return new SqlConnection(
            _configuration.GetConnectionString("DefaultConnection"));
    }
}