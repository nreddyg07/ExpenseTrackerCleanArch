using ExpenseTrackerCleanArch.Application.Features.Expenses.Commands.UpdateExpense;
using ExpenseTrackerCleanArch.Application.Interfaces;
using MediatR;

namespace ExpenseTrackerCleanArch.Application.Features.Expenses.Commands.UpdateExpense;

public class UpdateExpenseHandler : IRequestHandler<UpdateExpenseCommand, bool>
{
    private readonly IExpenseWriteRepository _repository;

    public UpdateExpenseHandler(IExpenseWriteRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(UpdateExpenseCommand request, CancellationToken ct)
    {
        var rowsAffected = await _repository.UpdateAsync(
            request.Id,
            request.Title,
            request.Amt,
            request.Category,
            request.Date,
            ct);

        return rowsAffected > 0;
    }
}
