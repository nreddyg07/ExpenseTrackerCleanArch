using ExpenseTrackerCleanArch.Application.Common.Responses;
using ExpenseTrackerCleanArch.Application.Interfaces;
using MediatR;

namespace ExpenseTrackerCleanArch.Application.Features.Expenses.Commands.DeleteMultipleExpense;

public class DeleteMultipleExpensesHandler
    : IRequestHandler<DeleteMultipleExpensesCommand, ApiResponse<string>>
{
    private readonly IExpenseWriteRepository _repository;

    public DeleteMultipleExpensesHandler(IExpenseWriteRepository repository)
    {
        _repository = repository;
    }

    public async Task<ApiResponse<string>> Handle(
        DeleteMultipleExpensesCommand request,
        CancellationToken cancellationToken)
    {
        if (request.Ids == null || !request.Ids.Any())
        {
            return ApiResponse<string>.FailResponse(
                "Invalid Request");
        }

        var deletedCount = await _repository.DeleteMultipleAsync(
            request.Ids,
            cancellationToken
        );

        if (deletedCount == 0)
        {
            return ApiResponse<string>.FailResponse(
                "Not Found");
        }

        return ApiResponse<string>.SuccessResponse(
            "Deleted",
            $"{deletedCount} expenses deleted successfully");
    }
}