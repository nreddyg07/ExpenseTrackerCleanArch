using Microsoft.EntityFrameworkCore;
using ExpenseTrackerCleanArch.Domain.Entities;
using ExpenseTrackerCleanArch.Application.Interfaces;

namespace ExpenseTrackerCleanArch.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<Expense> Expenses => Set<Expense>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Expense>()
            .Property(e => e.Amt)
            .HasPrecision(18, 2);
    }
}