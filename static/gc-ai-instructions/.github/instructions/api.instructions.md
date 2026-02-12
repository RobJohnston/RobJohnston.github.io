---
applyTo: "**/Controllers/**,**/Api/**,**/*Controller.cs,**/*ApiClient.cs"
---

# REST API Design Instructions

Government of Canada APIs must follow REST principles, be well-documented, secure, and accessible.

## Government of Canada Standards on APIs

All Government of Canada APIs must comply with the **[Standards on APIs](https://www.canada.ca/en/government/system/digital-government/digital-government-innovations/government-canada-standards-apis.html)**.

**Key requirements**:

1. **Architecture**: RESTful model by default, JSON message format (UTF-8), resource-oriented URLs
2. **Security**: TLS 1.2+, JWT for authentication, API keys in headers (not URLs), no sensitive data unencrypted
3. **Data Standards**: ISO 8601 datetime format in UTC (yyyy-mm-ddThh:mm:ssZ), JSON responses as objects (not arrays), consistent casing
4. **Versioning**: Format `v<Major>.<Minor>.<Patch>`, major version in URL (e.g., `/v3/`), support at least one previous major version
5. **Error Handling**: HTTP status codes, abstract internal details (no stack traces), consistent error response format
6. **Performance**: Pagination required for large result sets, restrict wildcard queries, published benchmarks
7. **Documentation**: OpenAPI specifications, published to API Store, include test data and examples

**This instruction file provides implementation guidance for these standards using ASP.NET Core.**

## API Design Principles

1. **Resource-oriented** - URLs represent resources, not actions
2. **HTTP methods** - Use standard verbs (GET, POST, PUT, PATCH, DELETE)
3. **Stateless** - Each request contains all necessary information
4. **Versioned** - APIs must be versioned for backwards compatibility
5. **Documented** - OpenAPI/Swagger documentation required
6. **Secure** - Authentication and authorization on all endpoints

## URL Structure

### Resource Naming

```
# ✅ GOOD: Plural nouns, lowercase, kebab-case
GET    /api/v1/applications
GET    /api/v1/applications/12345
GET    /api/v1/applications/12345/documents
POST   /api/v1/password-resets

# ❌ BAD: Verbs, mixed case, underscores
GET    /api/v1/getApplications
GET    /api/v1/Applications
GET    /api/v1/application_documents
POST   /api/v1/resetPassword
```

### HTTP Methods

```csharp
// GET - Retrieve resource(s)
[HttpGet("api/v1/applications")]
public async Task<ActionResult<IEnumerable<ApplicationDto>>> GetApplications()

[HttpGet("api/v1/applications/{id}")]
public async Task<ActionResult<ApplicationDto>> GetApplication(int id)

// POST - Create new resource
[HttpPost("api/v1/applications")]
public async Task<ActionResult<ApplicationDto>> CreateApplication([FromBody] CreateApplicationDto dto)

// PUT - Replace entire resource
[HttpPut("api/v1/applications/{id}")]
public async Task<ActionResult<ApplicationDto>> UpdateApplication(int id, [FromBody] UpdateApplicationDto dto)

// PATCH - Partial update
[HttpPatch("api/v1/applications/{id}")]
public async Task<ActionResult<ApplicationDto>> PatchApplication(int id, [FromBody] JsonPatchDocument<ApplicationDto> patch)

// DELETE - Remove resource
[HttpDelete("api/v1/applications/{id}")]
public async Task<IActionResult> DeleteApplication(int id)
```

## API Controller Pattern

```csharp
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace BenefitsApp.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Produces("application/json")]
    [Authorize] // Require authentication by default
    public class ApplicationsController : ControllerBase
    {
        private readonly IApplicationService _applicationService;
        private readonly ILogger<ApplicationsController> _logger;

        public ApplicationsController(
            IApplicationService applicationService,
            ILogger<ApplicationsController> logger)
        {
            _applicationService = applicationService;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all applications for the current user
        /// </summary>
        /// <returns>List of applications</returns>
        /// <response code="200">Returns the list of applications</response>
        /// <response code="401">If the user is not authenticated</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ApplicationDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<ApplicationDto>>> GetApplications()
        {
            var userId = User.FindFirst("sub")?.Value;
            var applications = await _applicationService.GetApplicationsByUserIdAsync(userId);
            return Ok(applications);
        }

        /// <summary>
        /// Retrieves a specific application by ID
        /// </summary>
        /// <param name="id">The application ID</param>
        /// <returns>The application</returns>
        /// <response code="200">Returns the application</response>
        /// <response code="404">If the application is not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApplicationDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApplicationDto>> GetApplication(int id)
        {
            var application = await _applicationService.GetApplicationByIdAsync(id);

            if (application == null)
                return NotFound();

            return Ok(application);
        }

        /// <summary>
        /// Creates a new application
        /// </summary>
        /// <param name="dto">The application data</param>
        /// <returns>The created application</returns>
        /// <response code="201">Returns the newly created application</response>
        /// <response code="400">If the request is invalid</response>
        [HttpPost]
        [ProducesResponseType(typeof(ApplicationDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApplicationDto>> CreateApplication([FromBody] CreateApplicationDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirst("sub")?.Value;
            var application = await _applicationService.CreateApplicationAsync(dto, userId);

            return CreatedAtAction(
                nameof(GetApplication),
                new { id = application.Id },
                application
            );
        }

        /// <summary>
        /// Updates an existing application
        /// </summary>
        /// <param name="id">The application ID</param>
        /// <param name="dto">The updated application data</param>
        /// <returns>The updated application</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApplicationDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApplicationDto>> UpdateApplication(
            int id,
            [FromBody] UpdateApplicationDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var application = await _applicationService.UpdateApplicationAsync(id, dto);

            if (application == null)
                return NotFound();

            return Ok(application);
        }

        /// <summary>
        /// Deletes an application
        /// </summary>
        /// <param name="id">The application ID</param>
        /// <returns>No content</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteApplication(int id)
        {
            var deleted = await _applicationService.DeleteApplicationAsync(id);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
```

## Data Transfer Objects (DTOs)

```csharp
using System.ComponentModel.DataAnnotations;

namespace BenefitsApp.Api.Models
{
    /// <summary>
    /// Application data returned from API
    /// </summary>
    public class ApplicationDto
    {
        public int Id { get; set; }
        public string ConfirmationNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime SubmissionDate { get; set; }
        public string Status { get; set; }
    }

    /// <summary>
    /// Data required to create a new application
    /// </summary>
    public class CreateApplicationDto
    {
        [Required(ErrorMessage = "First name is required")]
        [StringLength(100, ErrorMessage = "First name must not exceed 100 characters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(100, ErrorMessage = "Last name must not exceed 100 characters")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Social Insurance Number is required")]
        [RegularExpression(@"^\d{3}-\d{3}-\d{3}$", ErrorMessage = "SIN must be in format XXX-XXX-XXX")]
        public string Sin { get; set; }

        [Required]
        [Range(typeof(DateTime), "1900-01-01", "9999-12-31")]
        public DateTime DateOfBirth { get; set; }
    }

    /// <summary>
    /// Data for updating an existing application
    /// </summary>
    public class UpdateApplicationDto
    {
        [StringLength(100)]
        public string FirstName { get; set; }

        [StringLength(100)]
        public string LastName { get; set; }

        [EmailAddress]
        public string Email { get; set; }
    }
}
```

## Response Formatting

### Success Responses

```csharp
// 200 OK - Successful GET, PUT, PATCH
return Ok(data);

// 201 Created - Successful POST
return CreatedAtAction(nameof(GetResource), new { id = resource.Id }, resource);

// 204 No Content - Successful DELETE
return NoContent();
```

### Error Responses

```csharp
// 400 Bad Request - Validation errors
if (!ModelState.IsValid)
    return BadRequest(ModelState);

return BadRequest(new { error = "Invalid request data" });

// 401 Unauthorized - Not authenticated
return Unauthorized();

// 403 Forbidden - Authenticated but not authorized
return Forbid();

// 404 Not Found - Resource doesn't exist
return NotFound();

// 409 Conflict - Resource conflict
return Conflict(new { error = "Resource already exists" });

// 500 Internal Server Error - Unhandled exception
return StatusCode(500, new { error = "An error occurred" });
```

### Problem Details (RFC 7807)

```csharp
// Configure in Program.cs
builder.Services.AddProblemDetails();

// Use in controllers
return Problem(
    title: "Application not found",
    detail: $"Application with ID {id} does not exist",
    statusCode: StatusCodes.Status404NotFound
);

// Returns:
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.4",
  "title": "Application not found",
  "status": 404,
  "detail": "Application with ID 12345 does not exist",
  "traceId": "00-abc123..."
}
```

## Pagination

```csharp
public class PagedResult<T>
{
    public IEnumerable<T> Items { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    public bool HasPrevious => Page > 1;
    public bool HasNext => Page < TotalPages;
}

[HttpGet]
public async Task<ActionResult<PagedResult<ApplicationDto>>> GetApplications(
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 20)
{
    if (page < 1) page = 1;
    if (pageSize < 1 || pageSize > 100) pageSize = 20;

    var result = await _applicationService.GetApplicationsPagedAsync(page, pageSize);

    Response.Headers.Add("X-Total-Count", result.TotalCount.ToString());
    Response.Headers.Add("X-Page", result.Page.ToString());
    Response.Headers.Add("X-Page-Size", result.PageSize.ToString());

    return Ok(result);
}
```

## Filtering and Sorting

```csharp
[HttpGet]
public async Task<ActionResult<IEnumerable<ApplicationDto>>> GetApplications(
    [FromQuery] string status = null,
    [FromQuery] DateTime? submittedAfter = null,
    [FromQuery] DateTime? submittedBefore = null,
    [FromQuery] string sortBy = "submissionDate",
    [FromQuery] string sortOrder = "desc")
{
    var filter = new ApplicationFilter
    {
        Status = status,
        SubmittedAfter = submittedAfter,
        SubmittedBefore = submittedBefore,
        SortBy = sortBy,
        SortOrder = sortOrder
    };

    var applications = await _applicationService.GetApplicationsAsync(filter);
    return Ok(applications);
}

// Example request:
// GET /api/v1/applications?status=pending&submittedAfter=2025-01-01&sortBy=submissionDate&sortOrder=asc
```

## API Versioning

### URL Versioning (Recommended)

```csharp
// Install package
// dotnet add package Microsoft.AspNetCore.Mvc.Versioning

// Configure in Program.cs
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});

// Use in controllers
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class ApplicationsController : ControllerBase
{
    // v1 endpoints
}

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("2.0")]
public class ApplicationsV2Controller : ControllerBase
{
    // v2 endpoints
}
```

## OpenAPI/Swagger Documentation

```csharp
// Install packages
// dotnet add package Swashbuckle.AspNetCore

// Configure in Program.cs
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Benefits Application API",
        Version = "v1",
        Description = "API for submitting and managing Employment Insurance benefit applications",
        Contact = new OpenApiContact
        {
            Name = "Service Canada",
            Email = "support@servicecanada.gc.ca",
            Url = new Uri("https://www.canada.ca/en/employment-social-development.html")
        }
    });

    // Include XML comments
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);

    // Add JWT authentication
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Enable Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Benefits API v1");
        options.RoutePrefix = "api/docs"; // Access at /api/docs
    });
}
```

### Enable XML Documentation

```xml
<!-- In .csproj -->
<PropertyGroup>
    <GenerateDocumentationFile>true</GenerateDocumentationFile>
    <NoWarn>$(NoWarn);1591</NoWarn> <!-- Suppress missing XML comment warnings -->
</PropertyGroup>
```

## Authentication & Authorization

### JWT Bearer Authentication

```csharp
// Configure in Program.cs
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://auth.example.gc.ca";
        options.Audience = "benefits-api";
        options.RequireHttpsMetadata = true;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireClaim("role", "admin"));

    options.AddPolicy("CanViewApplications", policy =>
        policy.RequireClaim("permissions", "applications:read"));
});

// Use in controllers
[Authorize] // Require authentication
public class ApplicationsController : ControllerBase

[Authorize(Policy = "AdminOnly")] // Require specific policy
public class AdminController : ControllerBase

[Authorize(Roles = "Admin,Manager")] // Require specific roles
public class ManagementController : ControllerBase
```

## Rate Limiting

```csharp
// Install package
// dotnet add package AspNetCoreRateLimit

// Configure in Program.cs
builder.Services.AddMemoryCache();
builder.Services.Configure<IpRateLimitOptions>(options =>
{
    options.EnableEndpointRateLimiting = true;
    options.StackBlockedRequests = false;
    options.GeneralRules = new List<RateLimitRule>
    {
        new RateLimitRule
        {
            Endpoint = "*",
            Period = "1m",
            Limit = 60 // 60 requests per minute
        },
        new RateLimitRule
        {
            Endpoint = "POST:/api/*",
            Period = "1m",
            Limit = 10 // 10 POST requests per minute
        }
    };
});

builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
builder.Services.AddInMemoryRateLimiting();

app.UseIpRateLimiting();
```

## CORS Configuration

```csharp
// Configure in Program.cs
builder.Services.AddCors(options =>
{
    options.AddPolicy("GCPolicy", builder =>
    {
        builder
            .WithOrigins("https://www.canada.ca", "https://application.gc.ca")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

app.UseCors("GCPolicy");
```

## Caching

```csharp
[HttpGet("{id}")]
[ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
public async Task<ActionResult<ApplicationDto>> GetApplication(int id)
{
    var application = await _applicationService.GetApplicationByIdAsync(id);
    return Ok(application);
}

// Or use ETags for conditional requests
[HttpGet("{id}")]
public async Task<ActionResult<ApplicationDto>> GetApplication(int id)
{
    var application = await _applicationService.GetApplicationByIdAsync(id);

    var etag = GenerateETag(application);
    Response.Headers.Add("ETag", etag);

    if (Request.Headers["If-None-Match"] == etag)
        return StatusCode(304); // Not Modified

    return Ok(application);
}
```

## Health Checks

```csharp
// Configure in Program.cs
builder.Services.AddHealthChecks()
    .AddDbContextCheck<ApplicationDbContext>()
    .AddUrlGroup(new Uri("https://api.example.gc.ca/health"), name: "external-api");

app.MapHealthChecks("/health");
app.MapHealthChecks("/health/ready");
app.MapHealthChecks("/health/live");
```

## API Best Practices

### 1. Use Consistent Error Format

```csharp
public class ApiError
{
    public string Message { get; set; }
    public string Code { get; set; }
    public Dictionary<string, string[]> ValidationErrors { get; set; }
    public string TraceId { get; set; }
}
```

### 2. Include Request IDs

```csharp
public class RequestIdMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var requestId = context.Request.Headers["X-Request-ID"].FirstOrDefault()
            ?? Guid.NewGuid().ToString();

        context.Response.Headers.Add("X-Request-ID", requestId);
        await next(context);
    }
}
```

### 3. Log All Requests

```csharp
public class RequestLoggingMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        logger.LogInformation(
            "API Request: {Method} {Path} from {IP}",
            context.Request.Method,
            context.Request.Path,
            context.Connection.RemoteIpAddress
        );

        await next(context);

        logger.LogInformation(
            "API Response: {StatusCode} for {Method} {Path}",
            context.Response.StatusCode,
            context.Request.Method,
            context.Request.Path
        );
    }
}
```

### 4. Validate All Input

```csharp
// Use data annotations
[Required]
[StringLength(100)]
public string FirstName { get; set; }

// Or FluentValidation
public class CreateApplicationDtoValidator : AbstractValidator<CreateApplicationDto>
{
    public CreateApplicationDtoValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Sin).Matches(@"^\d{3}-\d{3}-\d{3}$");
    }
}
```

### 5. Return Appropriate Status Codes

- **200 OK**: Successful GET, PUT, PATCH
- **201 Created**: Successful POST
- **204 No Content**: Successful DELETE
- **400 Bad Request**: Validation error
- **401 Unauthorized**: Not authenticated
- **403 Forbidden**: Authenticated but not authorized
- **404 Not Found**: Resource not found
- **409 Conflict**: Resource conflict
- **429 Too Many Requests**: Rate limit exceeded
- **500 Internal Server Error**: Server error

## Common Mistakes to Avoid

❌ **Don't use verbs in URLs** - Use HTTP methods instead
❌ **Don't return HTML** - APIs should return JSON
❌ **Don't expose internal IDs** - Use UUIDs or obfuscated IDs
❌ **Don't skip validation** - Always validate input
❌ **Don't log sensitive data** - Never log passwords, tokens, SINs
❌ **Don't ignore versioning** - Version from day one
❌ **Don't skip authentication** - Secure all endpoints by default

## Resources

- **REST API Design Guidelines**: https://restfulapi.net/
- **Microsoft API Guidelines**: https://github.com/microsoft/api-guidelines
- **OpenAPI Specification**: https://swagger.io/specification/
- **Swashbuckle**: https://github.com/domaindrivendev/Swashbuckle.AspNetCore

## Version Information

This guidance is current for:
- ASP.NET Core 8.0
- Swashbuckle.AspNetCore 6.5+
- REST API standards

Last updated: 2025-02-11
