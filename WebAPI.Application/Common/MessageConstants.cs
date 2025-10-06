namespace WebAPI.Application.Common;

public static class MessageConstants
{
    // Success Messages
    public const string OperationSuccessful = "Operation completed successfully";
    public const string CreateSuccessful = "Record created successfully";
    public const string UpdateSuccessful = "Record updated successfully";
    public const string DeleteSuccessful = "Record deleted successfully";
    
    // Product Messages
    public const string ProductCreated = "Product created successfully";
    public const string ProductUpdated = "Product updated successfully";
    public const string ProductDeleted = "Product deleted successfully";
    public const string ProductNotFound = "Product not found";
    public const string ProductAlreadyExists = "Product with this name already exists";
    public const string ProductStockUpdated = "Product stock updated successfully";
    public const string ProductActivated = "Product activated successfully";
    public const string ProductDeactivated = "Product deactivated successfully";
    
    // Category Messages
    public const string CategoryCreated = "Category created successfully";
    public const string CategoryUpdated = "Category updated successfully";
    public const string CategoryDeleted = "Category deleted successfully";
    public const string CategoryNotFound = "Category not found";
    public const string CategoryAlreadyExists = "Category with this name already exists";
    public const string CategoryActivated = "Category activated successfully";
    public const string CategoryDeactivated = "Category deactivated successfully";
    public const string CategoryHasProducts = "Cannot delete category that contains products";
    
    // Validation Messages
    public const string ValidationFailed = "Validation failed";
    public const string RequiredField = "This field is required";
    public const string InvalidFormat = "Invalid format";
    public const string InvalidRange = "Value is out of valid range";
    public const string DuplicateValue = "Duplicate value not allowed";
    
    // Error Messages
    public const string InternalServerError = "An internal server error occurred";
    public const string UnauthorizedAccess = "Unauthorized access";
    public const string ForbiddenAccess = "Access forbidden";
    public const string BadRequest = "Bad request";
    public const string NotFound = "Resource not found";
    public const string Conflict = "Resource conflict";
    
    // Database Messages
    public const string DatabaseConnectionFailed = "Database connection failed";
    public const string TransactionFailed = "Transaction failed";
    public const string ConcurrencyConflict = "Record was modified by another user";
}