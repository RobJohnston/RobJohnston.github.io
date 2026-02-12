---
applyTo: "**/*.cs,**/Auth/**"
---

# Authentication Instructions

## JWT Bearer Authentication

```csharp
// Program.cs
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://login.microsoftonline.com/tenant-id";
        options.Audience = "api://app-id";
        options.RequireHttpsMetadata = true;
        
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
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
```

## Controller Usage

```csharp
[ApiController]
[Authorize] // Require authentication by default
public class ApplicationsController : ControllerBase
{
    [HttpGet]
    [Authorize(Policy = "CanViewApplications")]
    public async Task<ActionResult> GetApplications()
    {
        var userId = User.FindFirst("sub")?.Value;
        var apps = await _service.GetByUserIdAsync(userId);
        return Ok(apps);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult> CreateApplication([FromBody] CreateDto dto)
    {
        var app = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetApplication), new { id = app.Id }, app);
    }
}
```

## GCKey/Sign-In Integration

```csharp
builder.Services.AddAuthentication()
    .AddOpenIdConnect("GCKey", options =>
    {
        options.Authority = "https://gckey.gc.ca";
        options.ClientId = "your-client-id";
        options.ClientSecret = builder.Configuration["GCKey:ClientSecret"];
        options.ResponseType = "code";
        options.SaveTokens = true;
        
        options.Scope.Add("openid");
        options.Scope.Add("profile");
        options.Scope.Add("email");
    });
```

## Password Requirements (Protected B)

```csharp
builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 12;
    
    // Lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
    options.Lockout.MaxFailedAccessAttempts = 5;
    
    // User settings
    options.User.RequireUniqueEmail = true;
});
```

## Session Management (Protected B)

```csharp
builder.Services.AddSession(options =>
{
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // HTTPS only
    options.Cookie.HttpOnly = true; // No JavaScript access
    options.Cookie.SameSite = SameSiteMode.Strict; // CSRF protection
    options.IdleTimeout = TimeSpan.FromMinutes(15); // TBS requirement
});
```

Last updated: 2025-02-11
