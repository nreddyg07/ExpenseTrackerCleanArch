using ExpenseTrackerCleanArch.Application.Features.Expenses.DTOs;
using MediatR;

namespace ExpenseTrackerCleanArch.Application.Features.Expenses.Queries.GetAllExpenses;

public record GetAllExpensesQuery() : IRequest<List<ExpenseDto>>;