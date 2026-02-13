# Safety Guardrails

## Always Confirm Before Running

- `aws s3 rm --recursive` - Deletes S3 bucket contents (data loss)
- `az group delete` - Deletes entire Azure resource group (data loss)
- `dotnet ef database drop` - Drops database (permanent data loss)
- `git push --force` - Overwrites remote history
- `rm -rf` - Recursive file deletion

## Confirm in Production

- `dotnet publish` - Builds for production deployment
- `az webapp deploy` - Deploys application to Azure App Service
- `aws lambda update-function-code` - Updates AWS Lambda function
- `git push origin main` - Pushes to main branch (triggers CI/CD)
- `dotnet ef database update` - Runs database migrations (may affect production data)

## Safe to Run

- `az webapp log tail` - View logs (Azure)
- `aws cloudwatch tail` - View logs (AWS)
- `git status` - Show repository status
- `dotnet test` - Run tests
- `az resource list` - List Azure resources
