using FluentValidation;

namespace ExpenseTrackerCleanArch.Application.Features.Expenses.Commands.UpdateExpense;

public class UpdateExpenseValidator
    : AbstractValidator<UpdateExpenseCommand>
{
    public UpdateExpenseValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Invalid Expense Id");

        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Cannot be empty");

        RuleFor(x => x.Amt)
            .GreaterThan(0)
            .WithMessage("Should be greater than 0");

        RuleFor(x => x.Category)
            .NotEmpty()
            .WithMessage("Cannot be empty");
    }
}