using ExpenseTrackerCleanArch.Application.Features.Expenses.Commands.DeleteMultipleExpense;
using ExpenseTrackerCleanArch.Application.Interfaces;
using MediatR;

namespace ExpenseTrackerCleanArch.Application.Features.Expenses.Commands.DeleteMultipleExpense;

public class DeleteMultipleExpensesHandler : IRequestHandler<DeleteMultipleExpensesCommand, bool>
{
    private readonly IExpenseWriteRepository _repository;

    public DeleteMultipleExpensesHandler(IExpenseWriteRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteMultipleExpensesCommand request, CancellationToken ct)
    {
        if (request.Ids == null || !request.Ids.Any())
            return false;

        var rowsAffected = await _repository.DeleteMultipleAsync(request.Ids, ct);

        return rowsAffected > 0;
    }
}