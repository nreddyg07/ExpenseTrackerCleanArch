using ExpenseTrackerCleanArch.Application.Interfaces;
using MediatR;

namespace ExpenseTrackerCleanArch.Infrastructure.Persistence;

public class ExpenseServiceWrapper(IMediator mediator, IExpenseReadRepository queries) : IExpenseServiceWrapper
{
    public IMediator Mediator => mediator;
    public IExpenseReadRepository Queries => queries;
}