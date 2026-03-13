using ExpenseTrackerCleanArch.Application.Features.Expenses.DTOs;
using ExpenseTrackerCleanArch.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTrackerCleanArch.Application.Features.Expenses.Queries.GetAllExpenses;

public class GetAllExpensesHandler
    : IRequestHandler<GetAllExpensesQuery, List<ExpenseDto>>
{
    private readonly IApplicationDbContext _context;

    public GetAllExpensesHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<ExpenseDto>> Handle(GetAllExpensesQuery request, CancellationToken cancellationToken)
    {
        return await _context.Expenses
            .Select(x => new ExpenseDto
            {
                Id = x.Id,
                Title = x.Title,
                Amt = x.Amt,
                Category = x.Category,
                Date = x.Date
            })
            .ToListAsync(cancellationToken);
    }
}