using ExpenseTrackerCleanArch.Application.Common.Responses;
using ExpenseTrackerCleanArch.Application.Features.Expenses;
using ExpenseTrackerCleanArch.Application.Features.Expenses.Commands.UpdateExpense;
using ExpenseTrackerCleanArch.Application.Interfaces;
using MediatR;

public class UpdateExpenseHandler : IRequestHandler<UpdateExpenseCommand, ApiResponse<ExpenseDto>>
{
    private readonly IExpenseWriteRepository _repository;
    private readonly IExpenseReadRepository _readRepository;

    public UpdateExpenseHandler(
        IExpenseWriteRepository repository,
        IExpenseReadRepository readRepository)
    {
        _repository = repository;
        _readRepository = readRepository;
    }

    public async Task<ApiResponse<ExpenseDto>> Handle(
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

        var updated = await _readRepository.GetByIdAsync(request.Id, cancellationToken);

        return ApiResponse<ExpenseDto>.SuccessResponse(
            updated!,
            "Expense updated successfully");
    }
}