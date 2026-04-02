using ExpenseTrackerCleanArch.Application.Common.Responses;
using ExpenseTrackerCleanArch.Application.Features.Expenses;
using ExpenseTrackerCleanArch.Application.Features.Expenses.Commands.DeleteMultipleExpense;
using ExpenseTrackerCleanArch.Application.Interfaces;
using MediatR;

public class DeleteMultipleExpensesHandler
    : IRequestHandler<DeleteMultipleExpensesCommand, ApiResponse<IEnumerable<ExpenseDto>>>
{
    private readonly IExpenseWriteRepository _repository;
    private readonly IExpenseReadRepository _readRepository;

    public DeleteMultipleExpensesHandler(
        IExpenseWriteRepository repository,
        IExpenseReadRepository readRepository)
    {
        _repository = repository;
        _readRepository = readRepository;
    }

    public async Task<ApiResponse<IEnumerable<ExpenseDto>>> Handle(
        DeleteMultipleExpensesCommand request,
        CancellationToken cancellationToken)
    {
        if (request.Ids == null || !request.Ids.Any())
            return ApiResponse<IEnumerable<ExpenseDto>>.FailResponse("Invalid Request");

        var all = await _readRepository.GetAllAsync(cancellationToken);
        var toDelete = all.Where(x => request.Ids.Contains(x.Id)).ToList();

        await _repository.DeleteMultipleAsync(request.Ids, cancellationToken);

        return ApiResponse<IEnumerable<ExpenseDto>>.SuccessResponse(
            toDelete,
            "Expenses deleted successfully");
    }
}