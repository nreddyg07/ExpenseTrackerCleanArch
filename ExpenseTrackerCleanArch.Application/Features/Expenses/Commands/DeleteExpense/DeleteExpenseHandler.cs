using ExpenseTrackerCleanArch.Application.Common.Responses;
using ExpenseTrackerCleanArch.Application.Features.Expenses;
using ExpenseTrackerCleanArch.Application.Features.Expenses.Commands.DeleteExpense;
using ExpenseTrackerCleanArch.Application.Interfaces;
using MediatR;

public class DeleteExpenseHandler : IRequestHandler<DeleteExpenseCommand, ApiResponse<ExpenseDto>>
{
    private readonly IExpenseWriteRepository _repository;
    private readonly IExpenseReadRepository _readRepository;

    public DeleteExpenseHandler(
        IExpenseWriteRepository repository,
        IExpenseReadRepository readRepository)
    {
        _repository = repository;
        _readRepository = readRepository;
    }

    public async Task<ApiResponse<ExpenseDto>> Handle(
        DeleteExpenseCommand request,
        CancellationToken cancellationToken)
    {
        var existing = await _readRepository.GetByIdAsync(request.Id, cancellationToken);

        if (existing == null)
            return ApiResponse<ExpenseDto>.FailResponse("Expense not found");

        await _repository.DeleteAsync(request.Id, cancellationToken);

        return ApiResponse<ExpenseDto>.SuccessResponse(
            existing,
            "Expense deleted successfully");
    }
}