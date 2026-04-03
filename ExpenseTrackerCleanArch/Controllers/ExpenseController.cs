using ExpenseTrackerCleanArch.Application.Features.Expenses.DTOs;
using ExpenseTrackerCleanArch.Application.Features.Expenses;
using ExpenseTrackerCleanArch.Application.Features.Expenses.Commands.UpdateExpense;
using ExpenseTrackerCleanArch.Application.Features.Expenses.Commands.UpsertExpense;
using ExpenseTrackerCleanArch.Application.Features.Expenses.Commands.CreateExpense;
using ExpenseTrackerCleanArch.Application.Features.Expenses.Commands.DeleteExpense;
using ExpenseTrackerCleanArch.Application.Features.Expenses.Commands.DeleteMultipleExpense;
using ExpenseTrackerCleanArch.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]/[action]")]
public class ExpensesController(IExpenseServiceWrapper service) : ControllerBase
{
    private readonly IExpenseServiceWrapper _service=service;

    [HttpGet]
    public async Task<Ok<IEnumerable<ExpenseDto>>> GetAll(CancellationToken ct)
    {
        var data = await _service.Queries.GetAllAsync(ct);
        return TypedResults.Ok(data);
    }

    [HttpGet]
    public async Task<Results<Ok<ExpenseDto>, NotFound>> GetById([FromQuery]int id, CancellationToken ct)
    {
        var data = await _service.Queries.GetByIdAsync(id, ct);

        //if data is null, return 404
        if (data is null)
            return TypedResults.NotFound();

        return TypedResults.Ok(data);
    }

    [HttpPost]
    public async Task<Ok<bool>> Create([FromBody]CreateExpenseCommand command)
    {
        var result = await _service.Mediator.Send(command);
        return TypedResults.Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<Results<Ok<bool>, BadRequest<string>>> Update(int id, UpdateExpenseCommand command)
    {
        if (id != command.Id)
            return TypedResults.BadRequest("ID mismatch between URL and body.");

        var result = await _service.Mediator.Send(command);
        return TypedResults.Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<Ok<bool>> Delete(int id)
    {
        var result = await _service.Mediator.Send(new DeleteExpenseCommand(id));
        return TypedResults.Ok(result);
    }

    [HttpDelete("multipleDelete")]
    public async Task<Ok<bool>> DeleteMultiple([FromBody] List<int> ids)
    {
        var result = await _service.Mediator.Send(new DeleteMultipleExpensesCommand { Ids = ids });
        return TypedResults.Ok(result);
    }

    [HttpPost("upsert-multiple")]
    public async Task<Results<Ok<bool>, BadRequest<string>>> UpsertMultiple([FromBody] UpsertMultipleExpensesCommand command)
    {
        var success = await _service.Mediator.Send(command);

        if (!success)
            return TypedResults.BadRequest("No changes were made or invalid data provided.");

        return TypedResults.Ok(true);
    }
}