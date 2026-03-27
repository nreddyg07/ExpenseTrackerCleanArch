using ExpenseTrackerCleanArch.Application.Common.Responses;
using MediatR;

namespace ExpenseTrackerCleanArch.Application.Features.Expenses.Commands.UpdateExpense;

public record UpdateExpenseCommand(
    int Id,
    string Title,
    decimal Amt,
    string Category,
    DateTime Date
) : IRequest<ApiResponse<string>>;