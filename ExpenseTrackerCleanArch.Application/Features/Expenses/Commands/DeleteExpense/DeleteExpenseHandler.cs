using ExpenseTrackerCleanArch.Application.Common.Responses;
using ExpenseTrackerCleanArch.Application.Interfaces;
using MediatR;

namespace ExpenseTrackerCleanArch.Application.Features.Expenses.Commands.DeleteExpense;

public class DeleteExpenseHandler : IRequestHandler<DeleteExpenseCommand, ApiResponse<string>>
{
    private readonly IExpenseWriteRepository _repository;

    public DeleteExpenseHandler(IExpenseWriteRepository repository)
    {
        _repository = repository;
    }

    public async Task<ApiResponse<string>> Handle(
        DeleteExpenseCommand request,
        CancellationToken cancellationToken)
    {
        await _repository.DeleteAsync(request.Id, cancellationToken);

        return ApiResponse<string>.SuccessResponse(
            "Deleted",
            "Expense deleted successfully");
    }
}