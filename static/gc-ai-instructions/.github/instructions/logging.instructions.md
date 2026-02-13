---
applyTo: "**/*.cs,**/*.js,**/logging/**,**/logs/**"
---

# Logging Instructions

Proper logging is critical for debugging, monitoring, and security auditing in Government of Canada applications.

## Logging Principles

1. **Never log Protected B data** (PII, SINs, passwords, tokens)
2. **Use structured logging** for machine-readable logs
3. **Log at appropriate levels** (Trace, Debug, Information, Warning, Error, Critical)
4. **Include context** (request IDs, user IDs, operation names)
5. **Retain logs per TBS requirements** (90 days minimum for Protected B systems)

## Log Levels

| Level | When to Use | Example |
|-------|-------------|---------|
| **Trace** | Very detailed diagnostic information | Method entry/exit, loop iterations |
| **Debug** | Diagnostic information useful during development | Variable values, conditional branches |
| **Information** | General application flow | User logged in, application submitted, job completed |
| **Warning** | Unexpected but recoverable situations | Deprecated API used, fallback executed, retry attempted |
| **Error** | Operation failed but application continues | Failed to send email, database timeout, external API error |
| **Critical** | Application crash or data loss | Database unavailable, out of memory, security breach |

## .NET Logging Setup

### Configuration (appsettings.json)

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information",
      "BenefitsApp": "Information"
    }
  },
  "Serilog": {
    "Using": ["Serilog.Sinks.Console", "Serilog.Sinks.File"],
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning",
        "System": "Warning"
      }
    },
    "WriteTo": [
      {
        "Name": "Console",
        "Args": {
          "outputTemplate": "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}"
        }
      },
      {
        "Name": "File",
        "Args": {
          "path": "/var/log/benefits-app/log-.txt",
          "rollingInterval": "Day",
          "retainedFileCountLimit": 90,
          "outputTemplate": "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}"
        }
      }
    ],
    "Enrich": ["FromLogContext", "WithMachineName", "WithThreadId"],
    "Properties": {
      "Application": "BenefitsApp",
      "Environment": "Production"
    }
  }
}
```

### Serilog Setup (Recommended)

```csharp
// Install packages
// dotnet add package Serilog.AspNetCore
// dotnet add package Serilog.Sinks.Console
// dotnet add package Serilog.Sinks.File

// In Program.cs
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .Enrich.WithProperty("Application", "BenefitsApp")
    .Enrich.WithProperty("Environment", builder.Environment.EnvironmentName)
    .CreateLogger();

builder.Host.UseSerilog();

try
{
    Log.Information("Starting web application");
    var app = builder.Build();
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
```

## Structured Logging

### ✅ GOOD: Structured Logging

```csharp
// Use named placeholders
_logger.LogInformation(
    "User {UserId} submitted application {ApplicationId} at {Timestamp}",
    userId,
    applicationId,
    DateTime.UtcNow
);

// Output: User 12345 submitted application APP-67890 at 2025-02-11T10:30:00Z
// Structured: { "UserId": 12345, "ApplicationId": "APP-67890", "Timestamp": "2025-02-11T10:30:00Z" }
```

### ❌ BAD: String Interpolation

```csharp
// Don't use string interpolation or concatenation
_logger.LogInformation($"User {userId} submitted application {applicationId}");
// This loses the structured data - just a string!
```

## Protected B Compliance

### ❌ NEVER Log These

```csharp
// ❌ NEVER log PII
_logger.LogInformation("User email: {Email}", user.Email);

// ❌ NEVER log SINs
_logger.LogInformation("Processing SIN: {SIN}", application.Sin);

// ❌ NEVER log passwords
_logger.LogInformation("Login attempt: {Username}/{Password}", username, password);

// ❌ NEVER log tokens
_logger.LogInformation("Authorization: {Token}", authToken);

// ❌ NEVER log credit card numbers
_logger.LogInformation("Payment with card: {CardNumber}", cardNumber);
```

### ✅ Log These Instead

```csharp
// ✅ GOOD: Log internal IDs, not PII
_logger.LogInformation(
    "User {UserId} submitted application {ApplicationId}",
    user.Id, // Internal ID, not email or name
    application.Id
);

// ✅ GOOD: Hash IP addresses
_logger.LogInformation(
    "Login attempt from {IpAddressHash}",
    HashIpAddress(ipAddress)
);

// ✅ GOOD: Log success/failure, not sensitive data
_logger.LogInformation("Authentication successful for UserId {UserId}", userId);
_logger.LogWarning("Authentication failed for UserId {UserId}", userId);

// ✅ GOOD: Log masked data if necessary
_logger.LogInformation(
    "Payment processed with card ending in {CardLast4}",
    cardNumber.Substring(cardNumber.Length - 4)
);
```

## Common Logging Patterns

### Controller Logging

```csharp
[ApiController]
[Route("api/v1/[controller]")]
public class ApplicationsController : ControllerBase
{
    private readonly ILogger<ApplicationsController> _logger;

    [HttpPost]
    public async Task<ActionResult<ApplicationDto>> CreateApplication([FromBody] CreateApplicationDto dto)
    {
        _logger.LogInformation(
            "CreateApplication request received from UserId {UserId}",
            User.FindFirst("sub")?.Value
        );

        try
        {
            var result = await _applicationService.CreateApplicationAsync(dto);

            _logger.LogInformation(
                "Application {ApplicationId} created successfully",
                result.Id
            );

            return CreatedAtAction(nameof(GetApplication), new { id = result.Id }, result);
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning(
                ex,
                "Validation failed for CreateApplication request"
            );
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Unexpected error in CreateApplication"
            );
            return StatusCode(500);
        }
    }
}
```

### Service Logging

```csharp
public class ApplicationService : IApplicationService
{
    private readonly ILogger<ApplicationService> _logger;
    private readonly IApplicationRepository _repository;

    public async Task<Application> CreateApplicationAsync(CreateApplicationDto dto, string userId)
    {
        _logger.LogDebug(
            "CreateApplicationAsync called for UserId {UserId}",
            userId
        );

        var application = new Application
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            UserId = userId,
            SubmissionDate = DateTime.UtcNow
        };

        try
        {
            var id = await _repository.CreateAsync(application);
            application.Id = id;

            _logger.LogInformation(
                "Application {ApplicationId} created for UserId {UserId}",
                id,
                userId
            );

            return application;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(
                ex,
                "Database error creating application for UserId {UserId}",
                userId
            );
            throw;
        }
    }
}
```

### Database Logging

```csharp
public class ApplicationRepository : IApplicationRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ApplicationRepository> _logger;

    public async Task<int> CreateAsync(Application application)
    {
        try
        {
            _context.Applications.Add(application);
            await _context.SaveChangesAsync();

            _logger.LogDebug(
                "Application {ApplicationId} saved to database",
                application.Id
            );

            return application.Id;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(
                ex,
                "Failed to save application to database: {ErrorMessage}",
                ex.InnerException?.Message ?? ex.Message
            );
            throw;
        }
    }
}
```

## Request Logging Middleware

```csharp
public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var requestId = Guid.NewGuid().ToString();
        context.Items["RequestId"] = requestId;

        _logger.LogInformation(
            "HTTP {Method} {Path} started. RequestId: {RequestId}, IP: {IpAddressHash}",
            context.Request.Method,
            context.Request.Path,
            requestId,
            HashIpAddress(context.Connection.RemoteIpAddress?.ToString())
        );

        var sw = Stopwatch.StartNew();

        try
        {
            await _next(context);
        }
        finally
        {
            sw.Stop();

            _logger.LogInformation(
                "HTTP {Method} {Path} completed with {StatusCode} in {ElapsedMs}ms. RequestId: {RequestId}",
                context.Request.Method,
                context.Request.Path,
                context.Response.StatusCode,
                sw.ElapsedMilliseconds,
                requestId
            );
        }
    }

    private string HashIpAddress(string ipAddress)
    {
        if (string.IsNullOrEmpty(ipAddress)) return "unknown";

        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(ipAddress);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash).Substring(0, 8);
    }
}
```

## Azure Monitor / Application Insights

```csharp
// Install package
// dotnet add package Microsoft.ApplicationInsights.AspNetCore

// In Program.cs
builder.Services.AddApplicationInsightsTelemetry(options =>
{
    options.ConnectionString = builder.Configuration["ApplicationInsights:ConnectionString"];
});

// Usage in code
public class ApplicationService
{
    private readonly TelemetryClient _telemetry;

    public async Task ProcessApplicationAsync(int id)
    {
        using var operation = _telemetry.StartOperation<RequestTelemetry>("ProcessApplication");
        operation.Telemetry.Properties["ApplicationId"] = id.ToString();

        try
        {
            // Process application
            _telemetry.TrackEvent("ApplicationProcessed", new Dictionary<string, string>
            {
                { "ApplicationId", id.ToString() },
                { "Status", "Success" }
            });
        }
        catch (Exception ex)
        {
            _telemetry.TrackException(ex);
            throw;
        }
    }
}
```

## Log Retention

### TBS Requirements

- **Protected B systems**: Minimum 90 days
- **Production systems**: Recommended 365 days
- **Audit logs**: May require longer retention (check with security team)

### Configuration

```csharp
// Serilog file retention
"WriteTo": [
  {
    "Name": "File",
    "Args": {
      "path": "/var/log/benefits-app/log-.txt",
      "rollingInterval": "Day",
      "retainedFileCountLimit": 90  // 90 days
    }
  }
]
```

## Log Aggregation

### CloudWatch (AWS)

```csharp
// Install package
// dotnet add package AWS.Logger.Serilog

Log.Logger = new LoggerConfiguration()
    .WriteTo.AWSSink(
        configuration: builder.Configuration,
        textFormatter: new JsonFormatter()
    )
    .CreateLogger();
```

### Azure Monitor

```csharp
// Install package
// dotnet add package Serilog.Sinks.ApplicationInsights

Log.Logger = new LoggerConfiguration()
    .WriteTo.ApplicationInsights(
        configuration.GetValue<string>("ApplicationInsights:ConnectionString"),
        TelemetryConverter.Traces
    )
    .CreateLogger();
```

## Log Queries

### Find Errors

```csharp
// Azure Monitor / Application Insights (KQL)
traces
| where severityLevel >= 3  // Error and Critical
| where timestamp > ago(24h)
| project timestamp, severityLevel, message, customDimensions
| order by timestamp desc
```

### Track Application Flow

```csharp
traces
| where customDimensions.ApplicationId == "APP-12345"
| project timestamp, message, severityLevel
| order by timestamp asc
```

### Monitor Performance

```csharp
requests
| where timestamp > ago(1h)
| summarize avg(duration), max(duration), count() by name
| order by avg_duration desc
```

## Testing Logs

```csharp
[Fact]
public void CreateApplication_LogsInformation()
{
    // Arrange
    var loggerFactory = LoggerFactory.Create(builder => builder.AddDebug());
    var logger = loggerFactory.CreateLogger<ApplicationService>();
    var service = new ApplicationService(logger, _mockRepository.Object);

    // Act
    service.CreateApplicationAsync(new CreateApplicationDto());

    // Assert
    // Use a test logger that captures log messages
    // Verify log message was created with correct level and properties
}
```

## Common Mistakes

❌ **Don't log in loops** - Aggregate and log summary instead
❌ **Don't log entire objects** - Log specific properties
❌ **Don't use Console.WriteLine** - Use proper logger
❌ **Don't ignore exceptions** - Always log with exception object
❌ **Don't log synchronously in async methods** - Logging is already fast
❌ **Don't forget to configure log levels** - Set appropriate levels per environment

## Best Practices

✅ **Use log scopes for related operations**
✅ **Include correlation IDs**
✅ **Log at method boundaries (entry/exit)**
✅ **Use semantic logging** (structured data)
✅ **Monitor log volume** (too much or too little both bad)
✅ **Review logs regularly**
✅ **Set up alerts for errors**

## Resources

- **Serilog Documentation**: https://serilog.net/
- **Application Insights**: https://docs.microsoft.com/en-us/azure/azure-monitor/app/app-insights-overview
- **TBS Logging Requirements**: Internal security team documentation

Last updated: 2025-02-11
