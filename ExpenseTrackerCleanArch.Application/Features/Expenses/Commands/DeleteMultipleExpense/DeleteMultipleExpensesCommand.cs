using ExpenseTrackerCleanArch.Application.Common.Responses;
using MediatR;

namespace ExpenseTrackerCleanArch.Application.Features.Expenses.Commands.DeleteMultipleExpense;

public class DeleteMultipleExpensesCommand
    : IRequest<ApiResponse<string>>
{
    public List<int> Ids { get; set; } = new();
}