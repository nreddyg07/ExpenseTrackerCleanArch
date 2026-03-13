using MediatR;
using Microsoft.AspNetCore.Mvc;
using ExpenseTrackerCleanArch.Application.Features.Expenses.Commands.CreateExpense;
using ExpenseTrackerCleanArch.Application.Features.Expenses.Commands.UpdateExpense;
using ExpenseTrackerCleanArch.Application.Features.Expenses.Commands.DeleteExpense;
using ExpenseTrackerCleanArch.Application.Features.Expenses.Queries.GetExpenseById;
using ExpenseTrackerCleanArch.Application.Features.Expenses.Queries.GetAllExpenses;

namespace ExpenseTrackerCleanArch.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExpensesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ExpensesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateExpenseCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateExpenseCommand command)
    {
        if (id != command.Id)
            return BadRequest();

        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _mediator.Send(new DeleteExpenseCommand(id));
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetAllExpensesQuery());
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _mediator.Send(new GetExpenseByIdQuery(id));
        return Ok(result);
    }
}