using MediatR;

namespace ExpenseTrackerCleanArch.Application.Interfaces;

public interface IExpenseServiceWrapper
{
    IMediator Mediator { get; }
    IExpenseReadRepository Queries { get; }
}