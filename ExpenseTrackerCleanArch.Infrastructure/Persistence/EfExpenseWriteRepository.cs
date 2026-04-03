using ExpenseTrackerCleanArch.Application.Features.Expenses.DTOs;
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

    public async Task<int> AddAsync(string title, decimal amt, string category, DateTime date, CancellationToken ct)
    {
        var expense = new Expense { Title = title, Amt = amt, Category = category, Date = date };
        _context.Expenses.Add(expense);
        await _context.SaveChangesAsync(ct);
        return expense.Id; 
    }

    public async Task<int> UpdateAsync(int id, string title, decimal amt, string category, DateTime date, CancellationToken ct)
    {
        var expense = await _context.Expenses.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (expense == null) return 0;

        expense.Title = title;
        expense.Amt = amt;
        expense.Category = category;
        expense.Date = date;

        return await _context.SaveChangesAsync(ct);
    }

    public async Task<int> DeleteAsync(int id, CancellationToken ct)
    {
        var expense = await _context.Expenses.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (expense == null) return 0;

        _context.Expenses.Remove(expense);
        return await _context.SaveChangesAsync(ct);
    }

    public async Task<int> DeleteMultipleAsync(List<int> ids, CancellationToken ct)
    {
        var expenses = await _context.Expenses.Where(e => ids.Contains(e.Id)).ToListAsync(ct);
        if (!expenses.Any()) return 0;

        _context.Expenses.RemoveRange(expenses);
        return await _context.SaveChangesAsync(ct);
    }

    public async Task<int> AddMultipleAsync(IEnumerable<ExpenseDto> expenses, CancellationToken ct)
    {
        var entities = expenses.Select(e => new Expense { Title = e.Title, Amt = e.Amt, Category = e.Category, Date = e.Date }).ToList();
        await _context.Expenses.AddRangeAsync(entities, ct);
        return await _context.SaveChangesAsync(ct);
    }

    public async Task<int> UpdateMultipleAsync(IEnumerable<ExpenseDto> expenseDtos, CancellationToken ct)
    {
        var ids = expenseDtos.Select(x => x.Id).ToList();
        var existingExpenses = await _context.Expenses.Where(e => ids.Contains(e.Id)).ToListAsync(ct);
        if (!existingExpenses.Any()) return 0;

        foreach (var entity in existingExpenses)//use linq
        {
            var dto = expenseDtos.First(x => x.Id == entity.Id);
            entity.Title = dto.Title;
            entity.Amt = dto.Amt;
            entity.Category = dto.Category;
            entity.Date = dto.Date;
        }

        _context.Expenses.UpdateRange(existingExpenses);
        return await _context.SaveChangesAsync(ct);
    }
}