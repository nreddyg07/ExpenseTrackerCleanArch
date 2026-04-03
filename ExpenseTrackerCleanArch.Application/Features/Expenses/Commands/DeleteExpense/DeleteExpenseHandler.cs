using ExpenseTrackerCleanArch.Application.Features.Expenses.Commands.DeleteExpense;
using ExpenseTrackerCleanArch.Application.Interfaces;
using MediatR;

namespace ExpenseTrackerCleanArch.Application.Features.Expenses.Commands.DeleteExpense;

public class DeleteExpenseHandler : IRequestHandler<DeleteExpenseCommand, bool>
{
    private readonly IExpenseWriteRepository _repository;

    public DeleteExpenseHandler(IExpenseWriteRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteExpenseCommand request, CancellationToken ct)
    {
        var rowsAffected = await _repository.DeleteAsync(request.Id, ct);

        return rowsAffected > 0;
    }
}