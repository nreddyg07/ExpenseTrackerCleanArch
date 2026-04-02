using ExpenseTrackerCleanArch.Application.Common.Responses;
using ExpenseTrackerCleanArch.Application.Features.Expenses;
using ExpenseTrackerCleanArch.Application.Features.Expenses.Commands.CreateExpense;
using ExpenseTrackerCleanArch.Application.Features.Expenses.Commands.DeleteExpense;
using ExpenseTrackerCleanArch.Application.Features.Expenses.Commands.DeleteMultipleExpense;
using ExpenseTrackerCleanArch.Application.Features.Expenses.Commands.UpdateExpense;
using ExpenseTrackerCleanArch.Application.Features.Expenses.Commands.UpsertExpense;
using ExpenseTrackerCleanArch.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ExpensesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IExpenseReadRepository _expenseReadRepository;

    public ExpensesController(IMediator mediator, IExpenseReadRepository expenseReadRepository)
    {
        _mediator = mediator;
        _expenseReadRepository = expenseReadRepository;
    }

    [HttpGet]
    public async Task<ApiResponse<IEnumerable<ExpenseDto>>> GetAll(CancellationToken ct)
    {
        var data = await _expenseReadRepository.GetAllAsync(ct);

        return ApiResponse<IEnumerable<ExpenseDto>>.SuccessResponse(
            data, "Expenses retrieved successfully!"
        );
    }

    [HttpGet("{id}")]
    public async Task<ApiResponse<ExpenseDto>> GetById(int id, CancellationToken ct)
    {
        var data = await _expenseReadRepository.GetByIdAsync(id, ct);

        return data != null
            ? ApiResponse<ExpenseDto>.SuccessResponse(data, "Expense retrieved successfully!")
            : ApiResponse<ExpenseDto>.FailResponse("Expense not found.");
    }

    [HttpPost]
    public async Task<ApiResponse<ExpenseDto>> Create(CreateExpenseCommand command)
    {
        var result = await _mediator.Send(command);
        return result; // should return ExpenseDto
    }

    [HttpPut("{id}")]
    public async Task<ApiResponse<ExpenseDto>> Update(int id, UpdateExpenseCommand command)
    {
        id= command.Id;
        var result = await _mediator.Send(command);
        return result; // updated ExpenseDto
    }

    [HttpDelete("{id}")]
    public async Task<ApiResponse<ExpenseDto>> Delete(int id)
    {
        var result = await _mediator.Send(new DeleteExpenseCommand(id));
        return result; // return deleted ExpenseDto (optional but consistent)
    }

    [HttpDelete("multipleDelete")]
    public async Task<ApiResponse<IEnumerable<ExpenseDto>>> DeleteMultiple([FromBody] List<int> ids)
    {
        var result = await _mediator.Send(new DeleteMultipleExpensesCommand { Ids = ids });
        return result;
    }

    [HttpPost("upsertMultiple")]
    public async Task<ApiResponse<IEnumerable<ExpenseDto>>> UpsertMultiple([FromBody] UpsertMultipleExpensesCommand command)
    {
        var result = await _mediator.Send(command);
        return result;
    }
}