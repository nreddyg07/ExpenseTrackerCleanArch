using ExpenseTrackerCleanArch.Application.Common.Responses;
using MediatR;

namespace ExpenseTrackerCleanArch.Application.Features.Expenses.Commands.UpsertExpense;

public class UpsertMultipleExpensesCommand
    : IRequest<ApiResponse<IEnumerable<ExpenseDto>>>
{
    public List<ExpenseDto> Expenses { get; set; } = new();
}