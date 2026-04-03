using ExpenseTrackerCleanArch.Application.Features.Expenses.Commands.UpsertExpense;
using FluentValidation;

namespace ExpenseTrackerCleanArch.Application.Validators;

public class UpsertMultipleExpensesValidator : AbstractValidator<UpsertMultipleExpensesCommand>
{
    public UpsertMultipleExpensesValidator()
    {
        RuleFor(x => x.Expenses)
            .NotEmpty().WithMessage("The expense list cannot be empty.")
            .Must(x => x.Count <= 100).WithMessage("You cannot upsert more than 100 expenses at once.");

        RuleForEach(x => x.Expenses).ChildRules(expense =>
        {
            expense.RuleFor(e => e.Id)
                .GreaterThanOrEqualTo(0).WithMessage("Expense ID must be 0 or greater.");

            expense.RuleFor(e => e.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(100).WithMessage("Title cannot exceed 100 characters.");

            expense.RuleFor(e => e.Amt)
                .GreaterThan(0).WithMessage("Amount must be greater than zero.");

            expense.RuleFor(e => e.Category)
                .NotEmpty().WithMessage("Category is required.");
        });
    }
}