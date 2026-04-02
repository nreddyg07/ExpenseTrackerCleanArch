using ExpenseTrackerCleanArch.Application.Common.Responses;
using MediatR;

namespace ExpenseTrackerCleanArch.Application.Features.Expenses.Commands.CreateExpense;

public record CreateExpenseCommand(
    string Title,
    decimal Amt,
    string Category,
    DateTime Date
) : IRequest<ApiResponse<ExpenseDto>>;