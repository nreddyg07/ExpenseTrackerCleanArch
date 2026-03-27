//using ExpenseTrackerCleanArch.Application.Common.Exceptions;
//using ExpenseTrackerCleanArch.Application.Common.Responses;
//using ExpenseTrackerCleanArch.Application.Features.Expenses;

//using ExpenseTrackerCleanArch.Application.Interfaces;
//using MediatR;

//namespace ExpenseTrackerCleanArch.Application.Features.Expenses.Queries.GetExpenseById;

//public class GetExpenseByIdHandler : IRequestHandler<GetExpenseByIdQuery, ApiResponse<ExpenseDto>>
//{
//    private readonly IExpenseReadRepository _repository;

//    public GetExpenseByIdHandler(IExpenseReadRepository repository)
//    {
//        _repository = repository;
//    }

//    public async Task<ApiResponse<ExpenseDto>> Handle(
//        GetExpenseByIdQuery request,
//        CancellationToken cancellationToken)
//    {
//        var expense = await _repository.GetByIdAsync(request.Id, cancellationToken);

//        if (expense == null)
//            throw new NotFoundException($"Expense with id {request.Id} not found");

//        return ApiResponse<ExpenseDto>.SuccessResponse(
//            expense,
//            "Expense retrieved successfully");
//    }
//}