//using ExpenseTrackerCleanArch.Application.Common.Responses;
//using ExpenseTrackerCleanArch.Application.Features.Expenses;
//using ExpenseTrackerCleanArch.Application.Interfaces;
//using MediatR;

//namespace ExpenseTrackerCleanArch.Application.Features.Expenses.Queries.GetAllExpenses;

//public class GetAllExpensesHandler : IRequestHandler<GetAllExpensesQuery, ApiResponse<IEnumerable<ExpenseDto>>>
//{
//    private readonly IExpenseReadRepository _repository;

//    public GetAllExpensesHandler(IExpenseReadRepository repository)
//    {
//        _repository = repository;
//    }

//    public async Task<ApiResponse<IEnumerable<ExpenseDto>>> Handle(
//        GetAllExpensesQuery request,
//        CancellationToken cancellationToken)
//    {
//        var expenses = await _repository.GetAllAsync(cancellationToken);

//        return ApiResponse<IEnumerable<ExpenseDto>>.SuccessResponse(
//            expenses,
//            "Expenses retrieved successfully");
//    //}
//}