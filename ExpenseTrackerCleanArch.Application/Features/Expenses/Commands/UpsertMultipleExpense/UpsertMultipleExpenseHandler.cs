using ExpenseTrackerCleanArch.Application.Common.Responses;
using ExpenseTrackerCleanArch.Application.Features.Expenses.Commands.CreateExpense;
using ExpenseTrackerCleanArch.Application.Features.Expenses.Commands.UpdateExpense;
using ExpenseTrackerCleanArch.Application.Interfaces;
using MediatR;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ExpenseTrackerCleanArch.Application.Features.Expenses.Commands.UpsertExpense;

public class UpsertMultipleExpensesHandler
    : IRequestHandler<UpsertMultipleExpensesCommand, ApiResponse<string>>
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

    public async Task<ApiResponse<string>> Handle(
        UpsertMultipleExpensesCommand request,
        CancellationToken cancellationToken)
    {
        if (request.Expenses == null || !request.Expenses.Any())
        {
            return ApiResponse<string>.FailResponse("Invalid");
        }

        int created = 0;
        int updated = 0;

        foreach (var e in request.Expenses)
        {
            if (e.Id == 0)
            {
                // CREATE via existing handler
                var result = await _repoWrite.AddAsync(
                    e.Title,
                    e.Amt,
                    e.Category,
                    e.Date,
                    cancellationToken
                );
                created++;
            }
            else
            {
                var existing = await _readRepo.GetByIdAsync(
                    e.Id,
                    cancellationToken);

                if (existing != null)
                {
                    // UPDATE via existing handler
                    await _repoWrite.UpdateAsync(
                            e.Id,
                            e.Title,
                            e.Amt,
                            e.Category,
                            e.Date,
                        cancellationToken);
                    updated++;
                }
            }
        }

        return ApiResponse<string>.SuccessResponse(
            "Upsert Completed",
            $"Created: {created}, Updated: {updated}");
    }
}