using ExpenseTrackerCleanArch.Application.Common.Responses;
using ExpenseTrackerCleanArch.Application.Features.Expenses;
using ExpenseTrackerCleanArch.Application.Features.Expenses.Commands.UpsertExpense;
using ExpenseTrackerCleanArch.Application.Interfaces;
using MediatR;

public class UpsertMultipleExpensesHandler
    : IRequestHandler<UpsertMultipleExpensesCommand, ApiResponse<IEnumerable<ExpenseDto>>>
{
    private readonly IExpenseWriteRepository _repoWrite;
    private readonly IExpenseReadRepository _readRepo;

    public UpsertMultipleExpensesHandler(
        IExpenseWriteRepository repoWrite,
        IExpenseReadRepository readRepo)
    {
        _repoWrite = repoWrite;
        _readRepo = readRepo;
    }

    public async Task<ApiResponse<IEnumerable<ExpenseDto>>> Handle(
        UpsertMultipleExpensesCommand request,
        CancellationToken cancellationToken)
    {
        if (request.Expenses == null || !request.Expenses.Any())
            return ApiResponse<IEnumerable<ExpenseDto>>.FailResponse("Invalid");

        foreach (var e in request.Expenses)
        {
            if (e.Id == 0)
            {
                await _repoWrite.AddAsync(e.Title, e.Amt, e.Category, e.Date, cancellationToken);
            }
            else
            {
                await _repoWrite.UpdateAsync(e.Id, e.Title, e.Amt, e.Category, e.Date, cancellationToken);
            }
        }

        var updatedList = await _readRepo.GetAllAsync(cancellationToken);

        return ApiResponse<IEnumerable<ExpenseDto>>.SuccessResponse(
            updatedList,
            "Upsert completed");
    }
}