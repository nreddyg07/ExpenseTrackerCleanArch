using ExpenseTrackerCleanArch.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace ExpenseTrackerCleanArch.Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Expense> Expenses { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}