SELECT 
    Id,
    Title,
    Amt,
    Category,
    Date
FROM Expenses
WHERE Id = @Id;