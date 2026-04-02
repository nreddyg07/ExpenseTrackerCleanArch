using ExpenseTrackerCleanArch.Application.Common.Responses;
using ExpenseTrackerCleanArch.Application.Features.Expenses;
using ExpenseTrackerCleanArch.Application.Features.Expenses.Commands.CreateExpense;
using ExpenseTrackerCleanArch.Application.Interfaces;
using MediatR;

public class CreateExpenseHandler : IRequestHandler<CreateExpenseCommand, ApiResponse<ExpenseDto>>
{
    private readonly IExpenseWriteRepository _repository;
    private readonly IExpenseReadRepository _readRepository;

    public CreateExpenseHandler(
        IExpenseWriteRepository repository,
        IExpenseReadRepository readRepository)
    {
        _repository = repository;
        _readRepository = readRepository;
    }

    public async Task<ApiResponse<ExpenseDto>> Handle(
        CreateExpenseCommand request,
        CancellationToken cancellationToken)
    {
        var id = await _repository.AddAsync(
            request.Title,
            request.Amt,
            request.Category,
            request.Date,
            cancellationToken);

        var expense = await _readRepository.GetByIdAsync(id, cancellationToken);

        return ApiResponse<ExpenseDto>.SuccessResponse(
            expense!,
            "Expense created successfully");
    }
}