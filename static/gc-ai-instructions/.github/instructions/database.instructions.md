---
applyTo: "**/*.sql,**/models/**,**/repositories/**,**/migrations/**"
---

# Department Database Standards

These standards were established by the DBA team and are enforced in code reviews.

## Database Platform

**Primary database**: MS SQL Server 2019 (on-premises) / Azure SQL Database (cloud)
**Legacy systems**: Oracle 12c (being migrated to SQL Server)

Entity Framework Core supports both platforms via provider packages:
- SQL Server: `Microsoft.EntityFrameworkCore.SqlServer`
- Oracle: `Oracle.EntityFrameworkCore`

**Connection configuration** (in `appsettings.json`):
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=gc-sql-prod-01.dept.gc.ca;Database=BenefitsDB;Integrated Security=true;TrustServerCertificate=false;Encrypt=true;"
  }
}
```

**For cloud deployments** (Azure SQL Database via SSC):
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=tcp:dept-benefits-prod.database.windows.net,1433;Database=BenefitsDB;User ID=appuser;Password={password};Encrypt=true;TrustServerCertificate=false;Connection Timeout=30;"
  }
}
```

**For AWS RDS SQL Server** (via SSC):
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=dept-benefits-prod.abc123.ca-central-1.rds.amazonaws.com,1433;Database=BenefitsDB;User ID=appuser;Password={password};Encrypt=true;"
  }
}
```

## Entity Framework (EF) Core Usage

**Status**: APPROVED for new development (as of 2023)
- Use EF Core for data access (replaces raw SQL/stored procedures)
- Code-first migrations required for schema changes
- LINQ queries preferred over raw SQL

**When raw SQL is needed**:
- Complex reporting queries with performance requirements
- Bulk operations (use `ExecuteSqlRaw` with parameterization)
- Legacy stored procedure calls (during migration period)

## Naming Conventions

**Table names**:
- ~~Do NOT use `tbl_` prefix (deprecated as of 2023)~~
- Use PascalCase singular nouns: `Application`, `User`, `Benefit`
- Join tables: `EntityOneEntityTwo` (e.g., `UserRole`)

**Column names**:
- PascalCase: `FirstName`, `SubmissionDate`, `IsActive`
- Primary keys: `Id` (not `TableNameId`)
- Foreign keys: `EntityNameId` (e.g., `UserId`, `ApplicationId`)
- Avoid abbreviations unless industry-standard (e.g., `SIN` is OK, `FstNm` is not)

**EF Entity classes**:
```csharp
public class Application
{
    public int Id { get; set; }  // Primary key
    public string ConfirmationNumber { get; set; }
    public DateTime SubmissionDate { get; set; }

    // Foreign key
    public int UserId { get; set; }
    public User User { get; set; }  // Navigation property
}
```

## Query Requirements

**Always use parameterization** (prevents SQL injection):

```csharp
// ✅ GOOD: Parameterized query
var results = context.Applications
    .Where(a => a.UserId == userId)
    .ToList();

// ✅ GOOD: Parameterized raw SQL if needed
var results = context.Applications
    .FromSqlRaw("SELECT * FROM Applications WHERE UserId = {0}", userId)
    .ToList();

// ❌ BAD: String concatenation (SQL injection risk)
var sql = $"SELECT * FROM Applications WHERE UserId = {userId}";  // NEVER
```

**No SELECT \* in production**:
- Explicitly specify columns in raw SQL
- EF LINQ queries are OK (they generate explicit column lists)

## Migrations

**All schema changes via EF migrations**:

```bash
# Create migration
dotnet ef migrations add AddBenefitCalculationTable

# Review generated migration before applying
# Check: Migrations/YYYYMMDDHHMMSS_AddBenefitCalculationTable.cs

# Apply to database
dotnet ef database update
```

**Migration rules**:
- One logical change per migration
- Include both Up() and Down() methods
- Test rollback before committing
- Never modify existing migrations (create new ones)

## Performance Guidelines

**Eager loading** for related data:

```csharp
// ✅ GOOD: Eager load (one query)
var applications = context.Applications
    .Include(a => a.User)
    .Include(a => a.Benefits)
    .Where(a => a.Status == "Pending")
    .ToList();

// ❌ BAD: N+1 query problem
var applications = context.Applications
    .Where(a => a.Status == "Pending")
    .ToList();
// Each iteration below causes a separate query
foreach (var app in applications)
{
    var user = app.User;  // Lazy load - separate query per iteration
}
```

**Indexes**: Add for foreign keys and frequently queried columns:

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Application>()
        .HasIndex(a => a.ConfirmationNumber)
        .IsUnique();

    modelBuilder.Entity<Application>()
        .HasIndex(a => a.SubmissionDate);
}
```

## Code Review Checklist

Before submitting PRs with database changes:
- [ ] EF migrations included for schema changes
- [ ] All queries use parameterization (no string concatenation)
- [ ] Navigation properties defined for foreign keys
- [ ] Indexes added for new foreign keys
- [ ] Migration tested with both upgrade and rollback
- [ ] No raw SQL unless performance-justified (document reason)

## Questions?

Contact: database-team@department.gc.ca
Last updated: 2024-12-15 (reviewed quarterly)
