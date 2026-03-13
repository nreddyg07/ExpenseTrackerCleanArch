using ExpenseTrackerCleanArch.Application.Features.Expenses.DTOs;
using MediatR;

namespace ExpenseTrackerCleanArch.Application.Features.Expenses.Queries.GetExpenseById;

public record GetExpenseByIdQuery(int Id) : IRequest<ExpenseDto?>;