---
applyTo: "**/azure/**,**/infrastructure/**,**/bicep/**,**/arm/**"
---

# Azure Deployment Instructions (Protected B)

This application deploys to Microsoft Azure through SSC's Cloud Brokering Service framework agreement.

## Framework Agreement

Project uses Azure through SSC's Cloud Brokering Service framework agreement.

## Required Azure Services

- **Compute**: Azure App Service (Web Apps) or Azure Container Instances
- **Database**: Azure SQL Database with Transparent Data Encryption (TDE)
- **Storage**: Azure Storage Account with encryption at rest
- **Logging**: Azure Monitor / Application Insights with 90-day retention (TBS requirement)
- **Key Management**: Azure Key Vault for secrets and encryption keys
- **Authentication**: Azure Active Directory (Azure AD) integration

## Bicep Configuration

Azure uses Bicep (or ARM templates) for infrastructure as code.

### Resource Group

```bicep
// main.bicep
targetScope = 'subscription'

param location string = 'canadacentral'
param environment string = 'prod'
param projectName string = 'dept-benefits'

// Resource group for all resources
resource rg 'Microsoft.Resources/resourceGroups@2021-04-01' = {
  name: '${projectName}-${environment}-rg'
  location: location
  tags: {
    Environment: environment
    Project: projectName
    Classification: 'Protected B'
    ManagedBy: 'SSC Cloud Brokering'
  }
}
```

### Azure SQL Database

```bicep
// database.bicep
param location string
param sqlServerName string
param databaseName string
param administratorLogin string

@secure()
param administratorPassword string

// SQL Server
resource sqlServer 'Microsoft.Sql/servers@2022-05-01-preview' = {
  name: sqlServerName
  location: location
  properties: {
    administratorLogin: administratorLogin
    administratorLoginPassword: administratorPassword
    version: '12.0'
    minimalTlsVersion: '1.2'
    publicNetworkAccess: 'Disabled' // Protected B requirement
  }
}

// SQL Database
resource sqlDatabase 'Microsoft.Sql/servers/databases@2022-05-01-preview' = {
  parent: sqlServer
  name: databaseName
  location: location
  sku: {
    name: 'S1'
    tier: 'Standard'
  }
  properties: {
    collation: 'SQL_Latin1_General_CP1_CI_AS'
    maxSizeBytes: 268435456000 // 250 GB
    catalogCollation: 'SQL_Latin1_General_CP1_CI_AS'
    zoneRedundant: false
    readScale: 'Disabled'
    requestedBackupStorageRedundancy: 'Geo' // SSC requirement: geo-redundant backups
  }
}

// Transparent Data Encryption (Protected B requirement)
resource sqlDatabaseTDE 'Microsoft.Sql/servers/databases/transparentDataEncryption@2022-05-01-preview' = {
  parent: sqlDatabase
  name: 'current'
  properties: {
    state: 'Enabled'
  }
}

// Audit settings (Protected B requirement)
resource sqlServerAudit 'Microsoft.Sql/servers/auditingSettings@2022-05-01-preview' = {
  parent: sqlServer
  name: 'default'
  properties: {
    state: 'Enabled'
    retentionDays: 90 // TBS requirement: 90-day retention
    auditActionsAndGroups: [
      'SUCCESSFUL_DATABASE_AUTHENTICATION_GROUP'
      'FAILED_DATABASE_AUTHENTICATION_GROUP'
      'BATCH_COMPLETED_GROUP'
    ]
    isStorageSecondaryKeyInUse: false
    isAzureMonitorTargetEnabled: true
  }
}
```

### App Service (Web Application)

```bicep
// appservice.bicep
param location string
param appServicePlanName string
param webAppName string
param sqlConnectionString string

// App Service Plan
resource appServicePlan 'Microsoft.Web/serverfarms@2022-03-01' = {
  name: appServicePlanName
  location: location
  sku: {
    name: 'P1v3' // Production tier
    tier: 'PremiumV3'
    size: 'P1v3'
    capacity: 2 // Multiple instances for high availability
  }
  kind: 'linux'
  properties: {
    reserved: true // Required for Linux
  }
}

// Web App
resource webApp 'Microsoft.Web/sites@2022-03-01' = {
  name: webAppName
  location: location
  identity: {
    type: 'SystemAssigned' // Managed identity for Key Vault access
  }
  properties: {
    serverFarmId: appServicePlan.id
    httpsOnly: true // SSC requirement: HTTPS only
    siteConfig: {
      linuxFxVersion: 'DOTNETCORE|8.0'
      minTlsVersion: '1.2'
      ftpsState: 'Disabled'
      http20Enabled: true
      alwaysOn: true
      httpLoggingEnabled: true
      detailedErrorLoggingEnabled: true
      appSettings: [
        {
          name: 'ASPNETCORE_ENVIRONMENT'
          value: 'Production'
        }
        {
          name: 'APPINSIGHTS_INSTRUMENTATIONKEY'
          value: applicationInsights.properties.InstrumentationKey
        }
      ]
      connectionStrings: [
        {
          name: 'DefaultConnection'
          connectionString: sqlConnectionString
          type: 'SQLAzure'
        }
      ]
    }
  }
}

// Application Insights (logging and monitoring)
resource applicationInsights 'Microsoft.Insights/components@2020-02-02' = {
  name: '${webAppName}-insights'
  location: location
  kind: 'web'
  properties: {
    Application_Type: 'web'
    RetentionInDays: 90 // TBS requirement
    publicNetworkAccessForIngestion: 'Enabled'
    publicNetworkAccessForQuery: 'Enabled'
  }
}
```

### Azure Key Vault

```bicep
// keyvault.bicep
param location string
param keyVaultName string
param webAppPrincipalId string

// Key Vault
resource keyVault 'Microsoft.KeyVault/vaults@2023-02-01' = {
  name: keyVaultName
  location: location
  properties: {
    sku: {
      family: 'A'
      name: 'standard'
    }
    tenantId: subscription().tenantId
    enabledForDeployment: false
    enabledForDiskEncryption: false
    enabledForTemplateDeployment: true
    enableSoftDelete: true // SSC requirement
    softDeleteRetentionInDays: 90
    enablePurgeProtection: true // Protected B requirement
    publicNetworkAccess: 'Disabled' // Protected B requirement
    accessPolicies: [
      {
        tenantId: subscription().tenantId
        objectId: webAppPrincipalId
        permissions: {
          secrets: [
            'get'
            'list'
          ]
        }
      }
    ]
  }
}

// Store database connection string in Key Vault
resource sqlConnectionStringSecret 'Microsoft.KeyVault/vaults/secrets@2023-02-01' = {
  parent: keyVault
  name: 'SqlConnectionString'
  properties: {
    value: 'Server=tcp:...;Database=...;'
  }
}
```

### Storage Account

```bicep
// storage.bicep
param location string
param storageAccountName string

// Storage Account
resource storageAccount 'Microsoft.Storage/storageAccounts@2023-01-01' = {
  name: storageAccountName
  location: location
  sku: {
    name: 'Standard_GRS' // Geo-redundant storage (SSC requirement)
  }
  kind: 'StorageV2'
  properties: {
    accessTier: 'Hot'
    minimumTlsVersion: 'TLS1_2'
    supportsHttpsTrafficOnly: true // SSC requirement
    allowBlobPublicAccess: false // Protected B requirement
    encryption: {
      services: {
        blob: {
          enabled: true
          keyType: 'Account'
        }
        file: {
          enabled: true
          keyType: 'Account'
        }
      }
      keySource: 'Microsoft.Storage'
    }
    networkAcls: {
      defaultAction: 'Deny' // Protected B requirement
      bypass: 'AzureServices'
      virtualNetworkRules: []
      ipRules: []
    }
  }
}

// Blob container for application data
resource blobContainer 'Microsoft.Storage/storageAccounts/blobServices/containers@2023-01-01' = {
  name: '${storageAccount.name}/default/app-data'
  properties: {
    publicAccess: 'None'
  }
}
```

## Deployment via Azure DevOps

```yaml
# azure-pipelines.yml
trigger:
  branches:
    include:
      - main

pool:
  vmImage: 'ubuntu-latest'

variables:
  azureSubscription: 'SSC-BrokerService-Connection'
  resourceGroupName: 'dept-benefits-prod-rg'
  location: 'canadacentral'
  webAppName: 'dept-benefits-prod-app'

stages:
- stage: Build
  jobs:
  - job: BuildJob
    steps:
    - task: UseDotNet@2
      inputs:
        packageType: 'sdk'
        version: '8.x'

    - task: DotNetCoreCLI@2
      displayName: 'Restore NuGet packages'
      inputs:
        command: 'restore'
        projects: '**/*.csproj'

    - task: DotNetCoreCLI@2
      displayName: 'Build application'
      inputs:
        command: 'build'
        arguments: '--configuration Release'

    - task: DotNetCoreCLI@2
      displayName: 'Run tests'
      inputs:
        command: 'test'
        arguments: '--configuration Release --no-build'

    - task: DotNetCoreCLI@2
      displayName: 'Publish application'
      inputs:
        command: 'publish'
        publishWebProjects: true
        arguments: '--configuration Release --output $(Build.ArtifactStagingDirectory)'
        zipAfterPublish: true

    - task: PublishBuildArtifacts@1
      displayName: 'Publish artifacts'
      inputs:
        PathtoPublish: '$(Build.ArtifactStagingDirectory)'
        ArtifactName: 'drop'

- stage: Deploy
  dependsOn: Build
  condition: succeeded()
  jobs:
  - deployment: DeployJob
    environment: 'Production'
    strategy:
      runOnce:
        deploy:
          steps:
          - task: AzureWebApp@1
            displayName: 'Deploy to Azure App Service'
            inputs:
              azureSubscription: '$(azureSubscription)'
              appType: 'webAppLinux'
              appName: '$(webAppName)'
              package: '$(Pipeline.Workspace)/drop/**/*.zip'
              runtimeStack: 'DOTNETCORE|8.0'
```

## GC Cloud Guardrails Compliance

All Azure deployments must comply with [GC Cloud Guardrails](https://canada-ca.github.io/cloud-guardrails/):

### 1. Multi-Factor Authentication
- Enable MFA for all Azure AD accounts
- Use Conditional Access policies

### 2. Protect Data at Rest
- Enable Transparent Data Encryption (TDE) for SQL Database
- Enable encryption for Storage Accounts
- Use Azure Key Vault for secrets

### 3. Protect Data in Transit
- Enforce HTTPS only (no HTTP)
- Minimum TLS 1.2
- Disable FTP/FTPS

### 4. Enable Logging and Monitoring
- Application Insights for application logging
- Azure Monitor for infrastructure monitoring
- 90-day log retention (TBS requirement)
- Alert on security events

### 5. Network Security
- Use Private Endpoints for database access
- Disable public network access where possible
- Use Network Security Groups (NSG) to restrict traffic

### 6. Resource Tagging
```bicep
tags: {
  Environment: 'Production'
  Project: 'Benefits Application'
  Classification: 'Protected B'
  CostCenter: 'IT-12345'
  ManagedBy: 'SSC Cloud Brokering'
  Department: 'ESDC'
}
```

## Regional Requirements

- **Region**: Use `canadacentral` (Toronto) or `canadaeast` (Quebec) for Canadian data residency
- **Geo-Redundancy**: Enable geo-redundant storage and backups
- **High Availability**: Deploy across multiple availability zones

## Security Best Practices

1. **Never commit secrets** - Use Azure Key Vault
2. **Use managed identities** - Avoid connection strings in code
3. **Disable public access** - Use Private Endpoints
4. **Enable soft delete** - Protect against accidental deletion
5. **Implement RBAC** - Use least-privilege access
6. **Enable Azure Defender** - Security threat detection
7. **Regular security scans** - Use Azure Security Center
8. **Backup regularly** - Automated backups with geo-redundancy

## Connection Strings

### From Key Vault (Recommended)

```csharp
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        // Use managed identity to access Key Vault
        var keyVaultUrl = Configuration["KeyVault:Url"];
        var client = new SecretClient(new Uri(keyVaultUrl), new DefaultAzureCredential());

        // Retrieve connection string from Key Vault
        var connectionString = client.GetSecret("SqlConnectionString").Value.Value;

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));
    }
}
```

### Direct Configuration (Less Secure)

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=tcp:dept-benefits-prod.database.windows.net,1433;Database=BenefitsDB;Authentication=Active Directory Managed Identity;Encrypt=true;TrustServerCertificate=false;Connection Timeout=30;"
  }
}
```

## Cost Optimization

- Use Azure Reserved Instances for predictable workloads
- Implement auto-scaling for App Services
- Use Azure Storage tiers (Hot/Cool/Archive)
- Enable Azure Advisor recommendations
- Monitor with Azure Cost Management

## Monitoring and Alerts

```bicep
// Application Insights alert for high error rate
resource alertRule 'Microsoft.Insights/metricAlerts@2018-03-01' = {
  name: 'high-error-rate-alert'
  location: 'global'
  properties: {
    description: 'Alert when error rate exceeds 5%'
    severity: 2
    enabled: true
    scopes: [
      applicationInsights.id
    ]
    evaluationFrequency: 'PT5M'
    windowSize: 'PT15M'
    criteria: {
      'odata.type': 'Microsoft.Azure.Monitor.SingleResourceMultipleMetricCriteria'
      allOf: [
        {
          name: 'ErrorRate'
          metricName: 'exceptions/count'
          operator: 'GreaterThan'
          threshold: 5
          timeAggregation: 'Count'
        }
      ]
    }
    actions: [
      {
        actionGroupId: actionGroup.id
      }
    ]
  }
}
```

## Disaster Recovery

- **RTO**: 4 hours (Recovery Time Objective)
- **RPO**: 1 hour (Recovery Point Objective)
- **Backup Frequency**: Automated daily backups
- **Geo-Redundancy**: Enabled for all data stores
- **Failover Plan**: Documented in `docs/disaster-recovery.md`

## Support Contacts

- **SSC Cloud Brokering**: ssc-cloud-broker@servicecanada.gc.ca
- **Microsoft Azure Support**: Available through SSC framework agreement
- **Department Cloud Lead**: cloud-lead@department.gc.ca

## Further Reading

- [SSC Cloud Brokering Service](https://www.canada.ca/en/government/system/digital-government/digital-government-innovations/cloud-services.html)
- [GC Cloud Guardrails](https://canada-ca.github.io/cloud-guardrails/)
- [Azure Canada](https://azure.microsoft.com/en-ca/)
- [Azure SQL Database Security](https://docs.microsoft.com/en-us/azure/azure-sql/database/security-overview)
- [Azure Key Vault Best Practices](https://docs.microsoft.com/en-us/azure/key-vault/general/best-practices)

## Version Information

This guidance is current for:
- Azure Resource Manager (ARM) / Bicep
- Azure SQL Database
- Azure App Service
- .NET 8.0 LTS

Last updated: 2025-02-11
