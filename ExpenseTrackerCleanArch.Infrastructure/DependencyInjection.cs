using ExpenseTrackerCleanArch.Application.Interfaces;
using ExpenseTrackerCleanArch.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExpenseTrackerCleanArch.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IExpenseWriteRepository, EfExpenseWriteRepository>();

        services.AddScoped<IExpenseReadRepository, DapperExpenseReadRepository>();

        services.AddScoped<ISqlConnectionFactory, SqlConnectionFactory>();

        return services;
    }
}