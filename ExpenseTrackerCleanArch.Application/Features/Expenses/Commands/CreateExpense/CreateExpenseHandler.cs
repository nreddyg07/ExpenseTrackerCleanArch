using ExpenseTrackerCleanArch.Application.Common.Responses;
using ExpenseTrackerCleanArch.Application.Interfaces;
using MediatR;

namespace ExpenseTrackerCleanArch.Application.Features.Expenses.Commands.CreateExpense;

public class CreateExpenseHandler : IRequestHandler<CreateExpenseCommand, ApiResponse<int>>
{
    private readonly IExpenseWriteRepository _repository;

    public CreateExpenseHandler(IExpenseWriteRepository repository)
    {
        _repository = repository;
    }

    public async Task<ApiResponse<int>> Handle(
        CreateExpenseCommand request,
        CancellationToken cancellationToken)
    {
        var id = await _repository.AddAsync(
            request.Title,
            request.Amt,
            request.Category,
            request.Date,
            cancellationToken);

        return ApiResponse<int>.SuccessResponse(id, "Expense created successfully");
    }
}