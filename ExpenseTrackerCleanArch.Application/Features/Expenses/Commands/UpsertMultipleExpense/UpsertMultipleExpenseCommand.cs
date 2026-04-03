using ExpenseTrackerCleanArch.Application.Features.Expenses.DTOs;
using MediatR;

namespace ExpenseTrackerCleanArch.Application.Features.Expenses.Commands.UpsertExpense;

public record UpsertMultipleExpensesCommand(List<ExpenseDto> Expenses)
    : IRequest<bool>;