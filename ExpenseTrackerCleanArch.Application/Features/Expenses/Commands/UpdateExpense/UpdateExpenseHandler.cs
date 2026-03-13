using ExpenseTrackerCleanArch.Application.Common.Responses;
using ExpenseTrackerCleanArch.Application.Interfaces;
using MediatR;

namespace ExpenseTrackerCleanArch.Application.Features.Expenses.Commands.UpdateExpense;

public class UpdateExpenseHandler : IRequestHandler<UpdateExpenseCommand, ApiResponse<string>>
{
    private readonly IExpenseWriteRepository _repository;

    public UpdateExpenseHandler(IExpenseWriteRepository repository)
    {
        _repository = repository;
    }

    public async Task<ApiResponse<string>> Handle(
        UpdateExpenseCommand request,
        CancellationToken cancellationToken)
    {
        await _repository.UpdateAsync(
            request.Id,
            request.Title,
            request.Amt,
            request.Category,
            request.Date,
            cancellationToken);

        return ApiResponse<string>.SuccessResponse(
            "Updated",
            "Expense updated successfully");
    }
}