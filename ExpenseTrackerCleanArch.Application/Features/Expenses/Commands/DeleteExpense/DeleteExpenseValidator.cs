using ExpenseTrackerCleanArch.Application.Features.Expenses.Commands.DeleteExpense;
using FluentValidation;

namespace ExpenseTrackerCleanArch.Application.Features.Expenses.Commands.DeleteExpense;

public class DeleteExpenseValidator
    : AbstractValidator<DeleteExpenseCommand>
{
    public DeleteExpenseValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Invalid Expense Id");
    }
}