using ExpenseTrackerCleanArch.Application.Interfaces;
using ExpenseTrackerCleanArch.Domain.Entities;
using MediatR;

namespace ExpenseTrackerCleanArch.Application.Features.Expenses.Commands.CreateExpense;

public class CreateExpenseHandler : IRequestHandler<CreateExpenseCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateExpenseHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateExpenseCommand request, CancellationToken cancellationToken)
    {
        var expense = new Expense(
            request.Title,
            request.Amt,
            request.Category,
            request.Date
        );

        _context.Expenses.Add(expense);
        await _context.SaveChangesAsync(cancellationToken);

        return expense.Id;
    }
}