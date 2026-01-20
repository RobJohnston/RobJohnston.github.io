# Contact Form Azure Function

This Azure Function handles contact form submissions from the static site.

## Overview

The function receives POST requests with contact form data, validates the input, and sends an email via SMTP.

## Prerequisites

- Azure account
- Azure Functions Core Tools (for local development)
- SMTP server credentials (e.g., SendGrid, Gmail, Outlook)

## Configuration

### Local Development

1. Update `local.settings.json` with your SMTP credentials:
   ```json
   {
     "Values": {
       "SMTP_HOST": "smtp.sendgrid.net",
       "SMTP_PORT": "587",
       "SMTP_USER": "apikey",
       "SMTP_PASS": "your-sendgrid-api-key",
       "SMTP_ENABLE_SSL": "true",
       "TO_EMAIL": "rob@johnston.net"
     }
   }
   ```

2. Run locally:
   ```bash
   cd azure-function
   func start
   ```

### Azure Deployment

1. Create an Azure Function App:
   ```bash
   az group create --name ContactFormRG --location canadacentral
   az storage account create --name contactformstorage --location canadacentral --resource-group ContactFormRG --sku Standard_LRS
   az functionapp create --resource-group ContactFormRG --consumption-plan-location canadacentral --runtime dotnet --functions-version 4 --name robjohnston-contact-form --storage-account contactformstorage
   ```

2. Configure CORS in Azure Portal:
   - Go to your Function App > CORS
   - Add `https://robjohnston.github.io` to allowed origins
   - Save changes

3. Add Application Settings (environment variables):
   ```bash
   az functionapp config appsettings set --name robjohnston-contact-form --resource-group ContactFormRG --settings "SMTP_HOST=smtp.sendgrid.net"
   az functionapp config appsettings set --name robjohnston-contact-form --resource-group ContactFormRG --settings "SMTP_PORT=587"
   az functionapp config appsettings set --name robjohnston-contact-form --resource-group ContactFormRG --settings "SMTP_USER=apikey"
   az functionapp config appsettings set --name robjohnston-contact-form --resource-group ContactFormRG --settings "SMTP_PASS=your-sendgrid-api-key"
   az functionapp config appsettings set --name robjohnston-contact-form --resource-group ContactFormRG --settings "SMTP_ENABLE_SSL=true"
   az functionapp config appsettings set --name robjohnston-contact-form --resource-group ContactFormRG --settings "TO_EMAIL=rob@johnston.net"
   ```

4. Deploy the function:
   ```bash
   func azure functionapp publish robjohnston-contact-form
   ```

5. Update the Azure Function URL in `static/js/contact-form.js`:
   ```javascript
   const AZURE_FUNCTION_URL = 'https://robjohnston-contact-form.azurewebsites.net/api/contact';
   ```

## SMTP Provider Options

### SendGrid (Recommended)
- Free tier: 100 emails/day
- Sign up: https://sendgrid.com/
- Use API key as password with username "apikey"

### Gmail
- Host: smtp.gmail.com
- Port: 587
- Enable "Less secure app access" or use App Passwords

### Outlook/Office 365
- Host: smtp-mail.outlook.com
- Port: 587
- Use your Microsoft account credentials

## Testing

Test the function locally:

```bash
curl -X POST http://localhost:7071/api/contact \
  -H "Content-Type: application/x-www-form-urlencoded" \
  -d "fromEmail=test@example.com&message=Test message"
```

Test the deployed function:

```bash
curl -X POST https://robjohnston-contact-form.azurewebsites.net/api/contact \
  -H "Content-Type: application/x-www-form-urlencoded" \
  -d "fromEmail=test@example.com&message=Test message"
```

## Security Notes

- The function validates email format to prevent injection attacks
- SMTP credentials are stored securely in Azure Application Settings
- CORS is configured to only accept requests from your domain
- The function uses TLS/SSL for SMTP connections
- Rate limiting is handled by Azure Functions consumption plan

## Troubleshooting

- Check Azure Function logs in Application Insights
- Verify SMTP credentials are correct
- Ensure CORS settings include your domain
- Test SMTP connection separately if emails aren't sending
