---
applyTo: "**/*.cs,**/monitoring/**"
---

# Monitoring Instructions

Application monitoring and observability for Government of Canada applications.

## Application Insights

```csharp
// Program.cs
builder.Services.AddApplicationInsightsTelemetry(options =>
{
    options.ConnectionString = builder.Configuration["ApplicationInsights:ConnectionString"];
});

// Custom telemetry
public class ApplicationService
{
    private readonly TelemetryClient _telemetry;

    public async Task ProcessAsync(int id)
    {
        using var operation = _telemetry.StartOperation<RequestTelemetry>("ProcessApplication");
        operation.Telemetry.Properties["ApplicationId"] = id.ToString();

        try
        {
            // Process
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

## Health Checks

```csharp
builder.Services.AddHealthChecks()
    .AddDbContextCheck<ApplicationDbContext>("database")
    .AddUrlGroup(new Uri("https://api.gc.ca/health"), "external-api")
    .AddAzureKeyVault(new Uri("https://vault.azure.net/"), "keyvault");

app.MapHealthChecks("/health");
app.MapHealthChecks("/health/ready");
app.MapHealthChecks("/health/live");
```

## Metrics

```csharp
public class MetricsService
{
    private readonly ILogger<MetricsService> _logger;

    public void RecordApplicationSubmission()
    {
        _logger.LogInformation(
            "Metric: {MetricName}, Value: {Value}",
            "applications.submitted",
            1
        );
    }

    public void RecordProcessingTime(long ms)
    {
        _logger.LogInformation(
            "Metric: {MetricName}, Value: {Value}",
            "processing.time.ms",
            ms
        );
    }
}
```

## Azure Monitor Queries

```kusto
// Error rate
requests
| where timestamp > ago(24h)
| summarize total=count(), failures=countif(success == false) by bin(timestamp, 5m)
| extend errorRate = (failures * 100.0) / total

// Slow requests
requests
| where timestamp > ago(1h) and duration > 1000
| project timestamp, name, duration
| order by duration desc

// Top exceptions
exceptions
| where timestamp > ago(24h)
| summarize count() by type
| order by count_ desc
```

## Alerts

```bash
# Create alert for high error rate
az monitor metrics alert create \
  --name high-error-rate \
  --resource-group rg-name \
  --scopes /subscriptions/.../app \
  --condition "count exceptions/count > 10" \
  --window-size 5m
```

## Best Practices

✅ Monitor error rate, response time, availability
✅ Set up alerts for critical issues
✅ Include health checks
✅ Log structured data
✅ Track custom events
✅ Monitor dependencies

Last updated: 2025-02-11
