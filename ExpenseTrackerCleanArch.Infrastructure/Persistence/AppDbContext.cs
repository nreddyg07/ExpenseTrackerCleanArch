using ExpenseTrackerCleanArch.Application.Interfaces;
using ExpenseTrackerCleanArch.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTrackerCleanArch.Infrastructure.Persistence;

public class AppDbContext : DbContext, IApplicationDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Expense> Expenses => Set<Expense>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Fix decimal precision
        modelBuilder.Entity<Expense>()
            .Property(e => e.Amt)
            .HasPrecision(18, 2);  // 18 digits total, 2 after decimal
    }
}