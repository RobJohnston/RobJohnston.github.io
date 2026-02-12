# Deployment Troubleshooting Skill

## Purpose
Diagnose and resolve common Azure/AWS deployment failures for GC applications.

## Common Issues and Solutions

### Issue 1: Application Won't Start

**Symptoms**:
- HTTP 502/503 errors
- "Application failed to start" in logs
- Health check failures

**Diagnostic Steps**:
```bash
# Check application logs
az webapp log tail --name app-name --resource-group rg-name

# Or for AWS
aws logs tail /aws/appservice/app-name --follow
```

**Common Causes**:

1. **Missing environment variables**
```bash
# Check configured variables
az webapp config appsettings list --name app-name --resource-group rg-name

# Add missing variable
az webapp config appsettings set --name app-name --resource-group rg-name \
  --settings "ConnectionStrings__Default=Server=..."
```

2. **Wrong .NET version**
```bash
# Check configured runtime
az webapp config show --name app-name --resource-group rg-name \
  --query linuxFxVersion

# Update if needed
az webapp config set --name app-name --resource-group rg-name \
  --linux-fx-version "DOTNETCORE|8.0"
```

3. **Database connection failure**
- Check connection string format
- Verify firewall rules allow App Service IP
- Test connection from Azure Cloud Shell

### Issue 2: Database Connection Failures

**Symptoms**:
- "Unable to connect to database"
- Timeout errors
- Authentication failures

**Solutions**:

**Azure SQL**:
```bash
# Add App Service to firewall
az sql server firewall-rule create \
  --resource-group rg-name \
  --server sql-server-name \
  --name AllowAppService \
  --start-ip-address 0.0.0.0 \
  --end-ip-address 0.0.0.0

# Or use VNet integration (more secure)
az webapp vnet-integration add \
  --name app-name \
  --resource-group rg-name \
  --vnet vnet-name \
  --subnet subnet-name
```

**Connection String Issues**:
```csharp
// ✅ GOOD: Correct format
"Server=tcp:server.database.windows.net,1433;Database=db;User ID=user;Password=pass;Encrypt=true;"

// ❌ BAD: Missing Encrypt=true
"Server=server.database.windows.net;Database=db;User ID=user;Password=pass;"
```

### Issue 3: SSL/Certificate Errors

**Symptoms**:
- "Certificate validation failed"
- SSL handshake errors

**Solutions**:

1. **Trust server certificate (DEV ONLY)**:
```csharp
// Only for development!
var connectionString = $"{baseConnectionString};TrustServerCertificate=true";
```

2. **Install proper certificates**:
```bash
# Azure: Use managed certificates
az webapp config ssl bind \
  --name app-name \
  --resource-group rg-name \
  --certificate-thumbprint thumbprint \
  --ssl-type SNI
```

### Issue 4: Performance Issues

**Symptoms**:
- Slow response times
- Timeouts
- High CPU/memory usage

**Diagnostics**:
```bash
# Check metrics
az monitor metrics list \
  --resource /subscriptions/.../app-name \
  --metric "CpuPercentage" \
  --start-time 2025-02-11T00:00:00Z

# Enable Application Insights
az monitor app-insights component create \
  --app app-insights-name \
  --location canadacentral \
  --resource-group rg-name
```

**Common Fixes**:
- Scale up App Service plan
- Enable caching
- Optimize database queries
- Add CDN for static content

### Issue 5: Deployment Fails

**Symptoms**:
- Deployment stuck or fails
- Build errors
- Publish errors

**Solutions**:

1. **Check build logs**:
```bash
# Azure DevOps
az pipelines runs show --id run-id --open

# GitHub Actions
gh run view run-id --log
```

2. **Common build issues**:
```bash
# Missing dependencies
dotnet restore

# Wrong configuration
dotnet build --configuration Release

# Clean build directory
dotnet clean
rm -rf bin/ obj/
dotnet build
```

3. **Kudu deployment issues**:
```bash
# Access Kudu console
https://app-name.scm.azurewebsites.net

# Check deployment logs
https://app-name.scm.azurewebsites.net/api/deployments
```

### Issue 6: Authentication Errors

**Symptoms**:
- 401 Unauthorized constantly
- Token validation failures

**Solutions**:

1. **Check authentication configuration**:
```csharp
// Verify JWT settings
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://login.microsoftonline.com/tenant-id";
        options.Audience = "api://app-id";
    });
```

2. **Check Key Vault access**:
```bash
# Grant App Service access to Key Vault
az keyvault set-policy \
  --name key-vault-name \
  --object-id app-service-identity-id \
  --secret-permissions get list
```

## Quick Diagnostic Commands

**Azure**:
```bash
# Stream logs
az webapp log tail --name app-name --resource-group rg-name

# Restart app
az webapp restart --name app-name --resource-group rg-name

# Check health
curl https://app-name.azurewebsites.net/health

# View recent deployments
az webapp deployment list --name app-name --resource-group rg-name
```

**AWS**:
```bash
# View logs
aws logs tail /aws/appservice/app-name --follow

# Restart application
aws apprunner update-service --service-arn arn --force-new-deployment

# Check health
aws elbv2 describe-target-health --target-group-arn arn
```

## Prevention Tips

- ✅ Test deployments in staging first
- ✅ Use health checks
- ✅ Monitor Application Insights
- ✅ Set up alerts for errors
- ✅ Document environment variables
- ✅ Use managed identities instead of connection strings
- ✅ Enable diagnostic logging

Last updated: 2025-02-11
