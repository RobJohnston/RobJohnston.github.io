---
applyTo: "**/.github/workflows/**,**/azure-pipelines.yml"
---

# CI/CD Instructions

Continuous Integration and Deployment for Government of Canada applications.

## GitHub Actions

```yaml
name: Build and Deploy

on:
  push:
    branches: [ main ]
  pull_request:
    branches: [ main ]

jobs:
  build:
    runs-on: ubuntu-latest
    steps:
    - uses: actions/checkout@v3

    - name: Setup .NET
      uses: actions/setup-dotnet@v3
      with:
        dotnet-version: '8.0.x'

    - name: Restore
      run: dotnet restore

    - name: Build
      run: dotnet build --no-restore --configuration Release

    - name: Test
      run: dotnet test --no-build --configuration Release

    - name: Publish
      run: dotnet publish --no-build --configuration Release --output ./publish

    - name: Upload artifact
      uses: actions/upload-artifact@v3
      with:
        name: app
        path: ./publish

  deploy:
    needs: build
    runs-on: ubuntu-latest
    if: github.ref == 'refs/heads/main'
    steps:
    - name: Download artifact
      uses: actions/download-artifact@v3
      with:
        name: app

    - name: Deploy to Azure
      uses: azure/webapps-deploy@v2
      with:
        app-name: 'benefits-app'
        publish-profile: ${{ secrets.AZURE_WEBAPP_PUBLISH_PROFILE }}
```

## Azure DevOps

```yaml
trigger:
  branches:
    include:
      - main

pool:
  vmImage: 'ubuntu-latest'

stages:
- stage: Build
  jobs:
  - job: BuildJob
    steps:
    - task: UseDotNet@2
      inputs:
        version: '8.0.x'

    - task: DotNetCoreCLI@2
      displayName: 'Restore'
      inputs:
        command: 'restore'

    - task: DotNetCoreCLI@2
      displayName: 'Build'
      inputs:
        command: 'build'
        arguments: '--configuration Release'

    - task: DotNetCoreCLI@2
      displayName: 'Test'
      inputs:
        command: 'test'

    - task: DotNetCoreCLI@2
      displayName: 'Publish'
      inputs:
        command: 'publish'
        publishWebProjects: true
        arguments: '--output $(Build.ArtifactStagingDirectory)'

    - task: PublishBuildArtifacts@1
      inputs:
        PathtoPublish: '$(Build.ArtifactStagingDirectory)'

- stage: Deploy
  dependsOn: Build
  jobs:
  - deployment: DeployJob
    environment: 'Production'
    strategy:
      runOnce:
        deploy:
          steps:
          - task: AzureWebApp@1
            inputs:
              azureSubscription: 'Azure-Connection'
              appName: 'benefits-app'
```

## Best Practices

✅ Run tests on every PR
✅ Deploy only from main/master
✅ Use secrets for credentials
✅ Include accessibility tests
✅ Scan for vulnerabilities
✅ Use deployment slots for zero-downtime

Last updated: 2025-02-11
