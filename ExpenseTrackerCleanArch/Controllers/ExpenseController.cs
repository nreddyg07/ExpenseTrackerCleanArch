using ExpenseTrackerCleanArch.Application.Common.Responses;
using ExpenseTrackerCleanArch.Application.Features.Expenses;
using ExpenseTrackerCleanArch.Application.Features.Expenses.Commands.CreateExpense;
using ExpenseTrackerCleanArch.Application.Features.Expenses.Commands.DeleteExpense;
using ExpenseTrackerCleanArch.Application.Features.Expenses.Commands.DeleteMultipleExpense;
using ExpenseTrackerCleanArch.Application.Features.Expenses.Commands.UpdateExpense;
using ExpenseTrackerCleanArch.Application.Features.Expenses.Commands.UpsertExpense;
//using ExpenseTrackerCleanArch.Application.Features.Expenses.Queries.GetAllExpenses;
//using ExpenseTrackerCleanArch.Application.Features.Expenses.Queries.GetExpenseById;
using ExpenseTrackerCleanArch.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTrackerCleanArch.API.Controllers;

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
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var data = await _expenseReadRepository.GetAllAsync(ct);
        return Ok(ApiResponse<IEnumerable<ExpenseDto>>.SuccessResponse(data, "Expenses retrieved successfully!"));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var data = await _expenseReadRepository.GetByIdAsync(id, ct);

        return data != null
            ? Ok(ApiResponse<ExpenseDto>.SuccessResponse(data, "Expense retrieved successfully!"))
            : NotFound(ApiResponse<ExpenseDto>.FailResponse("Expense not found."));
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateExpenseCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }    

    [HttpPut]
    public async Task<IActionResult> Update(int id,UpdateExpenseCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _mediator.Send(new DeleteExpenseCommand(id));
        return Ok(result);
    }

    [HttpDelete("multipleDelete")]
    public async Task<IActionResult> deleteMultiple([FromBody] List<int> ids)
    {
        var result = await _mediator.Send(new DeleteMultipleExpensesCommand { Ids=ids});
        return Ok();
    }

    [HttpPost("upsertMultiple")]
    public async Task<IActionResult> UpsertMultiple([FromBody] UpsertMultipleExpensesCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}