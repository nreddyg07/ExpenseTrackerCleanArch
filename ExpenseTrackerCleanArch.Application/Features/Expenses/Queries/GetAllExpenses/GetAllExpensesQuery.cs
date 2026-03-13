using ExpenseTrackerCleanArch.Application.Common.Responses;
using MediatR;

namespace ExpenseTrackerCleanArch.Application.Features.Expenses.Queries.GetAllExpenses;

public record GetAllExpensesQuery()
    : IRequest<ApiResponse<IEnumerable<ExpenseDto>>>;