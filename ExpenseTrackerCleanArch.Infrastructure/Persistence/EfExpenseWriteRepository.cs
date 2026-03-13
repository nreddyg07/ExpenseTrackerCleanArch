using ExpenseTrackerCleanArch.Application.Interfaces;

using ExpenseTrackerCleanArch.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTrackerCleanArch.Infrastructure.Persistence;

public class EfExpenseWriteRepository : IExpenseWriteRepository
{
    private readonly AppDbContext _context;

    public EfExpenseWriteRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<int> AddAsync(
        string title,
        decimal amt,
        string category,
        DateTime date,
        CancellationToken cancellationToken)
    {
        var expense = new Expense
        {
            Title = title,
            Amt = amt,
            Category = category,
            Date = date
        };

        _context.Expenses.Add(expense);

        await _context.SaveChangesAsync(cancellationToken);

        return expense.Id;
    }

    public async Task UpdateAsync(
        int id,
        string title,
        decimal amt,
        string category,
        DateTime date,
        CancellationToken cancellationToken)
    {
        var expense = await _context.Expenses
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

        if (expense == null)
            throw new KeyNotFoundException($"Expense with id {id} not found");

        expense.Title = title;
        expense.Amt = amt;
        expense.Category = category;
        expense.Date = date;

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var expense = await _context.Expenses
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

        if (expense == null)
            throw new KeyNotFoundException($"Expense with id {id} not found");

        _context.Expenses.Remove(expense);

        await _context.SaveChangesAsync(cancellationToken);
    }
}