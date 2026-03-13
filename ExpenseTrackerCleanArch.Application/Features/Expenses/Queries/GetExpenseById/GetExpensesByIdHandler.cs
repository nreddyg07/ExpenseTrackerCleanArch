using ExpenseTrackerCleanArch.Application.Features.Expenses.DTOs;
using ExpenseTrackerCleanArch.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTrackerCleanArch.Application.Features.Expenses.Queries.GetExpenseById;

public class GetExpenseByIdHandler
    : IRequestHandler<GetExpenseByIdQuery, ExpenseDto?>
{
    private readonly IApplicationDbContext _context;

    public GetExpenseByIdHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ExpenseDto?> Handle(
        GetExpenseByIdQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.Expenses
            .Where(x => x.Id == request.Id)
            .Select(x => new ExpenseDto
            {
                Id = x.Id,
                Title = x.Title,
                Amt = x.Amt,
                Category = x.Category,
                Date = x.Date
            })
            .FirstOrDefaultAsync(cancellationToken);
    }
}