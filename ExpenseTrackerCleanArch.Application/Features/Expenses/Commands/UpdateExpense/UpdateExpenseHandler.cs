using ExpenseTrackerCleanArch.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTrackerCleanArch.Application.Features.Expenses.Commands.UpdateExpense;

public class UpdateExpenseHandler : IRequestHandler<UpdateExpenseCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public UpdateExpenseHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UpdateExpenseCommand request, CancellationToken cancellationToken)
    {
        var expense = await _context.Expenses
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (expense == null)
            return false;

        expense.Update(request.Title, request.Amt, request.Category, request.Date);

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}