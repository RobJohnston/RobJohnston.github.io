---
applyTo: "src/**/*.cs,src/**/*.js"
---

# Protected B Data Handling Instructions

This application handles Protected B information per TBS security classification.

## What is Protected B?

Information that could cause serious injury to individuals or organizations if compromised. Examples:
- Social Insurance Numbers (SIN)
- Medical records
- Financial information
- Personal contact information combined with identifiers

## Security Compliance Requirements

Protected B applications must follow:

1. **TBS Security Policy** - Treasury Board Secretariat security classification and controls
2. **CCCS ITSG-33** - [IT Security Risk Management framework](https://www.cyber.gc.ca/en/guidance/it-security-risk-management-lifecycle-approach-itsg-33) from the Canadian Centre for Cyber Security
3. **CCCS CMVP** - [Cryptographic Module Validation Program](https://www.cyber.gc.ca/en/cryptographic-module-validation-program) for encryption at rest
4. **GC Cloud Guardrails** - If deployed to cloud, follow [mandatory baseline security controls](https://github.com/canada-ca/cloud-guardrails)
5. **CCCS Cloud Security Profile** - If using cloud services, follow [cloud security guidance](https://www.cyber.gc.ca/en/guidance/cloud-security-guidance)

**Key controls from CCCS ITSG-33**:
- **AC (Access Control)**: Role-based access, multi-factor authentication, session management
- **AU (Audit and Accountability)**: Comprehensive logging without PII, 2-year retention
- **SC (System and Communications Protection)**: TLS 1.2+, data encryption at rest (CMVP-validated modules)
- **IA (Identification and Authentication)**: Strong authentication, password complexity, account lockout

## Logging Requirements

**CRITICAL**: Never log Protected B information. Use structured logging with PII redaction:

```csharp
using Microsoft.Extensions.Logging;
using System.Text.Json;

/// <summary>
/// Log an event with structured data (no PII).
///
/// GC Security:
///     - TBS IT Security Framework: AU-2 (Audit Events)
///     - Protected B: No PII in logs
/// </summary>
public class AuditLogger
{
    private readonly ILogger<AuditLogger> _logger;

    public AuditLogger(ILogger<AuditLogger> logger)
    {
        _logger = logger;
    }

    public void LogEvent(string eventType, Dictionary<string, object> data)
    {
        var logEntry = new
        {
            Timestamp = DateTime.UtcNow,
            EventType = eventType,
            Data = data
        };

        _logger.LogInformation("{@LogEntry}", logEntry);
    }
}

// ✅ GOOD: Log events without PII
auditLogger.LogEvent("user_login_success", new Dictionary<string, object>
{
    { "user_id", "user-123" },      // Use internal ID, not SIN or email
    { "session_id", "sess-abc" },
    { "ip_address_hash", HashIp(httpContext.Connection.RemoteIpAddress) }  // Hash, don't log raw IP
});

// ❌ BAD: Do not log PII
_logger.LogInformation($"User {email} with SIN {sin} logged in");  // NEVER DO THIS
```

## Data Storage

Protected B data must be encrypted at rest:

```csharp
using System.Security.Cryptography;
using Microsoft.AspNetCore.DataProtection;

/// <summary>
/// Store Protected B data with encryption.
///
/// GC Security:
///     - TBS IT Security Framework: SC-28 (Protection of Information at Rest)
///     - Protected B: Encryption required
/// </summary>
public class SensitiveDataService
{
    private readonly IDataProtector _protector;
    private readonly ApplicationDbContext _context;

    public SensitiveDataService(
        IDataProtectionProvider provider,
        ApplicationDbContext context)
    {
        // Load encryption key from Key Management Service (NOT in code)
        _protector = provider.CreateProtector("SensitiveData");
        _context = context;
    }

    public async Task StoreSensitiveData(string sin, string userId)
    {
        // Encrypt the SIN
        var encryptedSin = _protector.Protect(sin);

        // Store encrypted value in database
        var sensitiveData = new UserSensitive
        {
            UserId = userId,
            EncryptedSin = encryptedSin
        };

        _context.UserSensitive.Add(sensitiveData);
        await _context.SaveChangesAsync();
    }

    public async Task<string> RetrieveSensitiveData(string userId)
    {
        // Retrieve encrypted data
        var result = await _context.UserSensitive
            .FirstOrDefaultAsync(u => u.UserId == userId);

        if (result == null)
            return null;

        // Decrypt and return
        return _protector.Unprotect(result.EncryptedSin);
    }
}
```

**Important**:
- Encryption keys must be managed via cloud provider KMS (AWS KMS, Azure Key Vault, etc.)
- Never hard-code encryption keys in source code
- Rotate encryption keys according to TBS IT Security Framework schedule

## Session Management

Protected B applications require secure session handling:

```csharp
// In Startup.cs or Program.cs

/// <summary>
/// Configure session for Protected B applications.
///
/// GC Security:
///     - TBS IT Security Framework: SC-10 (Network Disconnect)
///     - Protected B: 15-minute inactivity timeout required
/// </summary>
public void ConfigureServices(IServiceCollection services)
{
    services.AddSession(options =>
    {
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;  // HTTPS only
        options.Cookie.HttpOnly = true;  // No JavaScript access
        options.Cookie.SameSite = SameSiteMode.Strict;  // CSRF protection
        options.IdleTimeout = TimeSpan.FromMinutes(15);  // 15 minutes (TBS requirement)
        options.Cookie.IsEssential = true;
    });

    services.AddDistributedMemoryCache();
}

public void Configure(IApplicationBuilder app)
{
    app.UseSession();  // Session middleware automatically resets timeout on each request
}
```

## Access Control

Implement role-based access control (RBAC):

```csharp
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

/// <summary>
/// Attribute to enforce role-based access control.
///
/// GC Security:
///     - TBS IT Security Framework: AC-3 (Access Enforcement)
///     - Protected B: Least privilege principle
/// </summary>
public class RequireRoleAttribute : ActionFilterAttribute
{
    private readonly string _requiredRole;
    private readonly ILogger<RequireRoleAttribute> _logger;

    public RequireRoleAttribute(string requiredRole)
    {
        _requiredRole = requiredRole;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var httpContext = context.HttpContext;
        var userRole = httpContext.Session.GetString("user_role");
        var userId = httpContext.Session.GetString("user_id");

        if (userRole != _requiredRole)
        {
            // Log unauthorized access attempt
            var logger = httpContext.RequestServices
                .GetService<ILogger<RequireRoleAttribute>>();

            logger.LogWarning(
                "Unauthorized access attempt: UserId={UserId}, RequiredRole={RequiredRole}, UserRole={UserRole}",
                userId, _requiredRole, userRole);

            context.Result = new ForbidResult();  // 403 Forbidden
        }

        base.OnActionExecuting(context);
    }
}

// Usage
[HttpGet("admin/users")]
[RequireRole("admin")]
public IActionResult AdminUsers()
{
    // Only users with 'admin' role can access
    return View("AdminUsers");
}
```

## Input Validation

Always validate and sanitize user input to prevent injection attacks:

```csharp
using System.Text.RegularExpressions;
using System.Web;

/// <summary>
/// Validate Canadian Social Insurance Number format.
///
/// GC Security:
///     - TBS IT Security Framework: SI-10 (Information Input Validation)
///     - Protected B: Prevent injection attacks
/// </summary>
public bool ValidateSin(string sin)
{
    // Remove any whitespace or dashes
    var sinCleaned = Regex.Replace(sin, @"[\s-]", "");

    // Must be exactly 9 digits
    if (!Regex.IsMatch(sinCleaned, @"^\d{9}$"))
        return false;

    // Validate using Luhn algorithm (SIN checksum)
    return ValidateLuhn(sinCleaned);
}

/// <summary>
/// Sanitize user input for display (prevent XSS).
///
/// GC Security:
///     - TBS IT Security Framework: SI-10 (Information Input Validation)
///     - OWASP Top 10: XSS Prevention
/// </summary>
public string SanitizeUserInput(string userInput)
{
    return HttpUtility.HtmlEncode(userInput);
}
```

## Database Queries (SQL Injection Prevention)

Always use Entity Framework or parameterized queries:

```csharp
// ✅ GOOD: Entity Framework (prevents SQL injection)
/// <summary>
/// GC Security:
///     - TBS IT Security Framework: SI-10 (Information Input Validation)
///     - OWASP Top 10: SQL Injection Prevention
/// </summary>
public async Task<User> GetUserByEmail(string email)
{
    return await _context.Users
        .FirstOrDefaultAsync(u => u.Email == email);
}

// ✅ GOOD: Parameterized raw SQL if EF not sufficient
public async Task<User> GetUserByEmailRaw(string email)
{
    return await _context.Users
        .FromSqlInterpolated($"SELECT * FROM Users WHERE Email = {email}")
        .FirstOrDefaultAsync();
}

// ❌ BAD: String concatenation (vulnerable to SQL injection)
public async Task<User> GetUserByEmailBad(string email)
{
    var sql = $"SELECT * FROM Users WHERE Email = '{email}'";  // NEVER DO THIS
    return await _context.Users.FromSqlRaw(sql).FirstOrDefaultAsync();
}
```

## Security Headers

All responses must include security headers:

```csharp
/// <summary>
/// Middleware to set security headers for Protected B compliance.
///
/// GC Security:
///     - TBS IT Security Framework: SC-8 (Transmission Confidentiality)
///     - Protected B: HTTPS enforcement and XSS protection
/// </summary>
public class SecurityHeadersMiddleware
{
    private readonly RequestDelegate _next;

    public SecurityHeadersMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        context.Response.Headers.Add("Strict-Transport-Security",
            "max-age=31536000; includeSubDomains");
        context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
        context.Response.Headers.Add("X-Frame-Options", "DENY");
        context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
        context.Response.Headers.Add("Content-Security-Policy",
            "default-src 'self'; script-src 'self' www.canada.ca");

        await _next(context);
    }
}

// In Startup.cs or Program.cs
public void Configure(IApplicationBuilder app)
{
    app.UseMiddleware<SecurityHeadersMiddleware>();
    // ... other middleware
}
```

## Incident Response

If a security incident occurs (unauthorized access, data breach, etc.):

1. **Immediately notify**: Contact ITSCA team at itsca-team@example.gc.ca
2. **Log the incident**: Include timestamp, user ID, IP address (hashed), action attempted
3. **Preserve evidence**: Do not delete logs or modify system state
4. **Follow TBS Incident Response Plan**: See `docs/incident-response-plan.md`

## Testing Checklist

Before deploying Protected B code:
- [ ] No PII in logs (run: `npm run test:pii-detection`)
- [ ] All sensitive data encrypted at rest
- [ ] Session timeout set to 15 minutes
- [ ] Security headers present on all responses
- [ ] Input validation on all user inputs
- [ ] Parameterized queries for all database access
- [ ] Role-based access control enforced
- [ ] HTTPS enforced (no HTTP allowed)

## Common Mistakes to Avoid

❌ **Don't log PII**—use internal IDs and hashed values
❌ **Don't store passwords in plaintext**—use bcrypt or similar
❌ **Don't trust user input**—always validate and sanitize
❌ **Don't use string interpolation in SQL**—use parameterized queries
❌ **Don't hard-code secrets**—use environment variables
❌ **Don't skip session timeouts**—15 minutes required for Protected B
