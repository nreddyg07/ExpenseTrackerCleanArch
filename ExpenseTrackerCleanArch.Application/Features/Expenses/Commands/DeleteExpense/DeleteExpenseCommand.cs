using MediatR;

namespace ExpenseTrackerCleanArch.Application.Features.Expenses.Commands.DeleteExpense;

public record DeleteExpenseCommand(int Id)
    : IRequest<bool>;