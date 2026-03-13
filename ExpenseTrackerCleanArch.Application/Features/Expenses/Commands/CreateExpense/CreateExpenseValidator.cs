using FluentValidation;

namespace ExpenseTrackerCleanArch.Application.Features.Expenses.Commands.CreateExpense;

public class CreateExpenseValidator
    : AbstractValidator<CreateExpenseCommand>
{
    public CreateExpenseValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Title is required");

        RuleFor(x => x.Amt)
            .GreaterThan(0)
            .WithMessage("Amount must be greater than 0");

        RuleFor(x => x.Category)
            .NotEmpty()
            .WithMessage("Category is required");
    }
}