using ExpenseTrackerCleanArch.Application.Interfaces;
using ExpenseTrackerCleanArch.Infrastructure.Persistence;
using ExpenseTrackerCleanArch.Infrastructure.Persistence.QueryHelpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExpenseTrackerCleanArch.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Pass configuration to SqlConnectionFactory
        services.AddScoped<ISqlConnectionFactory>(_ => new SqlConnectionFactory(configuration));

        services.AddScoped<IQueryContext, QueryContext>();
        services.AddScoped<IQueryHelper, QueryHelper>();

        services.AddScoped<IExpenseReadRepository, DapperExpenseReadRepository>();
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IExpenseWriteRepository, EfExpenseWriteRepository>();

        return services;
    }
}