using ExpenseTrackerCleanArch.Application.Features.Expenses;
using ExpenseTrackerCleanArch.Application.Common.Responses;
using MediatR;

namespace ExpenseTrackerCleanArch.Application.Features.Expenses.Queries.GetExpenseById;

public record GetExpenseByIdQuery(int Id)
    : IRequest<ApiResponse<ExpenseDto>>;