using ExpenseTrackerCleanArch.Application.Features.Expenses.DTOs;
using ExpenseTrackerCleanArch.Application.Features.Expenses.Commands.UpsertExpense;
using ExpenseTrackerCleanArch.Domain.Entities;
using ExpenseTrackerCleanArch.Application.Interfaces;
using MediatR;

namespace ExpenseTrackerCleanArch.Application.Features.Expenses.Commands.UpsertExpense;

public class UpsertMultipleExpensesHandler
    : IRequestHandler<UpsertMultipleExpensesCommand, bool>
{
    private readonly IExpenseWriteRepository _repoWrite;

    public UpsertMultipleExpensesHandler(IExpenseWriteRepository repoWrite)
    {
        _repoWrite = repoWrite;
    }

    public async Task<bool> Handle(UpsertMultipleExpensesCommand request, CancellationToken ct)
    {
        if (request.Expenses == null || !request.Expenses.Any())
            return false;

        var toAdd = request.Expenses.Where(e => e.Id == 0).ToList();
        var toUpdate = request.Expenses.Where(e => e.Id != 0).ToList();

        int rowsAffected = 0;

        if (toAdd.Any())
            rowsAffected += await _repoWrite.AddMultipleAsync(toAdd, ct);

        if (toUpdate.Any())
            rowsAffected += await _repoWrite.UpdateMultipleAsync(toUpdate, ct);

        return rowsAffected > 0;
    }
}