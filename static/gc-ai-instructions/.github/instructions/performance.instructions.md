---
applyTo: "**/*.cs"
---

# Performance Optimization Instructions

## Database Performance

### Avoid N+1 Queries

```csharp
// ✅ GOOD: Eager loading
var apps = await _context.Applications
    .Include(a => a.User)
    .Include(a => a.Documents)
    .ToListAsync();

// ❌ BAD: N+1 queries
var apps = await _context.Applications.ToListAsync();
foreach (var app in apps)
{
    var user = await _context.Users.FindAsync(app.UserId); // N queries!
}
```

### Use Pagination

```csharp
var items = await query
    .Skip((page - 1) * pageSize)
    .Take(pageSize)
    .ToListAsync();
```

### Add Indexes

```csharp
modelBuilder.Entity<Application>()
    .HasIndex(a => a.UserId);
    
modelBuilder.Entity<Application>()
    .HasIndex(a => new { a.Status, a.SubmissionDate }); // Composite
```

## Caching

```csharp
[ResponseCache(Duration = 60)]
public async Task<ActionResult> GetApplication(int id)
{
    return Ok(await _service.GetByIdAsync(id));
}

// Distributed cache
private readonly IDistributedCache _cache;

public async Task<T> GetCachedAsync<T>(string key, Func<Task<T>> factory)
{
    var cached = await _cache.GetStringAsync(key);
    if (cached != null)
        return JsonSerializer.Deserialize<T>(cached);

    var value = await factory();
    await _cache.SetStringAsync(key, JsonSerializer.Serialize(value));
    return value;
}
```

## Async Best Practices

```csharp
// ✅ GOOD
public async Task<Application> ProcessAsync(int id)
{
    var app = await _repository.GetByIdAsync(id);
    await _service.ProcessAsync(app);
    return app;
}

// ❌ BAD: Blocking
public Application Process(int id)
{
    return _repository.GetByIdAsync(id).Result; // Deadlock risk!
}
```

Last updated: 2025-02-11
