using ExpenseTrackerCleanArch.Application.Common.Responses;
using MediatR;

namespace ExpenseTrackerCleanArch.Application.Features.Expenses.Commands.DeleteMultipleExpense;

public class DeleteMultipleExpensesCommand
    : IRequest<ApiResponse<IEnumerable<ExpenseDto>>>
{
    public List<int> Ids { get; set; } = new();
}