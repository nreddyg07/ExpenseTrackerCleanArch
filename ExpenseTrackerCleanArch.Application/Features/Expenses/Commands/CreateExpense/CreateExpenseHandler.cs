using ExpenseTrackerCleanArch.Application.Features.Expenses.Commands.CreateExpense;
using ExpenseTrackerCleanArch.Application.Interfaces;
using MediatR;

namespace ExpenseTrackerCleanArch.Application.Features.Expenses.Commands.CreateExpense;

public class CreateExpenseHandler : IRequestHandler<CreateExpenseCommand, bool>
{
    private readonly IExpenseWriteRepository _repository;

    public CreateExpenseHandler(IExpenseWriteRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(CreateExpenseCommand request, CancellationToken ct)
    {
        var rowsAffected = await _repository.AddAsync(
            request.Title,
            request.Amt,
            request.Category,
            request.Date,
            ct);

        return rowsAffected > 0;
    }
}