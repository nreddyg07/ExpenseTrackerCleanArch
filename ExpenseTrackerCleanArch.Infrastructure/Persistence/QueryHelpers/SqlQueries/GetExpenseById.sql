SELECT 
    Id,
    Title,
    Amt AS Amount,
    Category,
    Date
FROM Expenses
WHERE Id = @Id;