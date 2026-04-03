using ExpenseTrackerCleanArch.Application.Features.Expenses.Commands.DeleteMultipleExpense;
using FluentValidation;

namespace ExpenseTrackerCleanArch.Application.Validators
{
    public class DeleteMultipleExpenseValidator
        : AbstractValidator<DeleteMultipleExpensesCommand>
    {
        public DeleteMultipleExpenseValidator()
        {
            RuleFor(x => x.Ids)
                .NotEmpty()
                .WithMessage("At least one Expense Id must be provided.");
            RuleForEach(x => x.Ids)
                .GreaterThan(0)
                .WithMessage("Invalid Expense Id found in the list.");
        }
    }
}