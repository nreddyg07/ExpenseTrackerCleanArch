using MediatR;

namespace ExpenseTrackerCleanArch.Application.Features.Expenses.Commands.DeleteMultipleExpense;

public class DeleteMultipleExpensesCommand
    : IRequest<bool>
{
    public List<int> Ids { get; set; } = new();
}