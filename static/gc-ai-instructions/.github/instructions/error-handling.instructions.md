---
applyTo: "**/*.cs,**/exceptions/**"
---

# Error Handling Instructions

Proper error handling improves reliability and user experience.

## Exception Strategy

```csharp
// Custom base exception
public class BenefitsAppException : Exception
{
    public string ErrorCode { get; set; }
    
    public BenefitsAppException(string message, string errorCode = null) 
        : base(message)
    {
        ErrorCode = errorCode;
    }
}

// Domain exceptions
public class ValidationException : BenefitsAppException
{
    public Dictionary<string, string[]> Errors { get; set; }
    
    public ValidationException(Dictionary<string, string[]> errors) 
        : base("Validation failed", "VALIDATION_ERROR")
    {
        Errors = errors;
    }
}

public class NotFoundException : BenefitsAppException
{
    public NotFoundException(string resource, object id) 
        : base($"{resource} {id} not found", "NOT_FOUND") { }
}
```

## Global Exception Handler

```csharp
public class GlobalExceptionMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        _logger.LogError(ex, "Unhandled exception");

        var (statusCode, response) = ex switch
        {
            ValidationException ve => (400, new { error = ve.Message, errors = ve.Errors }),
            NotFoundException nfe => (404, new { error = nfe.Message }),
            UnauthorizedException => (401, new { error = "Unauthorized" }),
            _ => (500, new { error = "Internal server error" })
        };

        context.Response.StatusCode = statusCode;
        await context.Response.WriteAsJsonAsync(response);
    }
}
```

## Best Practices

✅ Log before rethrowing
✅ Include context (RequestId, UserId)
✅ Use specific exceptions
✅ Return appropriate HTTP status codes
✅ Never expose stack traces to users

Last updated: 2025-02-11
