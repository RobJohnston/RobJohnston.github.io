---
applyTo: "**/*.cs,**/*.js,**/notifications/**,**/email/**"
---

# GC Notify Instructions

GC Notify is the Government of Canada's notification service for sending emails and text messages. All GC applications should use GC Notify instead of custom email/SMS implementations.

## Overview

**Service**: https://notification.canada.ca/
**Documentation**: https://documentation.notification.canada.ca/

**Benefits**:
- Bilingual by default
- Accessible email templates
- Delivery tracking and analytics
- No infrastructure to maintain
- Free for government departments
- Compliant with Official Languages Act

## Getting Started

### 1. Register Your Service

1. Go to https://notification.canada.ca/
2. Create an account with your @gc.ca email
3. Create a service (one per application)
4. Get API keys (Team and whitelist, Live)

### 2. Install Client Library

```bash
# .NET
dotnet add package Notify

# Node.js/JavaScript
npm install notifications-node-client
```

### 3. Configuration

**appsettings.json**:
```json
{
  "GCNotify": {
    "ApiKey": "your-api-key-here",
    "BaseUrl": "https://api.notification.canada.ca",
    "Templates": {
      "ApplicationConfirmation": "template-id-1",
      "StatusUpdate": "template-id-2",
      "PasswordReset": "template-id-3"
    }
  }
}
```

**IMPORTANT**: Never commit API keys to source control. Use:
- Azure Key Vault for production
- User Secrets for development: `dotnet user-secrets set "GCNotify:ApiKey" "your-key"`

## .NET Implementation

### Service Setup

```csharp
using Notify.Client;
using Notify.Models;

public interface IGCNotifyService
{
    Task<string> SendEmailAsync(string templateId, string emailAddress, Dictionary<string, dynamic> personalisation);
    Task<string> SendSmsAsync(string templateId, string phoneNumber, Dictionary<string, dynamic> personalisation);
}

public class GCNotifyService : IGCNotifyService
{
    private readonly NotificationClient _client;
    private readonly ILogger<GCNotifyService> _logger;

    public GCNotifyService(IConfiguration configuration, ILogger<GCNotifyService> logger)
    {
        var apiKey = configuration["GCNotify:ApiKey"];
        var baseUrl = configuration["GCNotify:BaseUrl"];

        if (string.IsNullOrEmpty(apiKey))
            throw new InvalidOperationException("GC Notify API key is not configured");

        _client = new NotificationClient(baseUrl, apiKey);
        _logger = logger;
    }

    public async Task<string> SendEmailAsync(
        string templateId,
        string emailAddress,
        Dictionary<string, dynamic> personalisation)
    {
        try
        {
            var response = await _client.SendEmailAsync(
                emailAddress,
                templateId,
                personalisation
            );

            _logger.LogInformation(
                "Email sent via GC Notify: NotificationId={NotificationId}, Template={TemplateId}",
                response.id,
                templateId
            );

            return response.id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email via GC Notify: Template={TemplateId}", templateId);
            throw;
        }
    }

    public async Task<string> SendSmsAsync(
        string templateId,
        string phoneNumber,
        Dictionary<string, dynamic> personalisation)
    {
        try
        {
            var response = await _client.SendSmsAsync(
                phoneNumber,
                templateId,
                personalisation
            );

            _logger.LogInformation(
                "SMS sent via GC Notify: NotificationId={NotificationId}, Template={TemplateId}",
                response.id,
                templateId
            );

            return response.id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send SMS via GC Notify: Template={TemplateId}", templateId);
            throw;
        }
    }
}
```

### Dependency Injection

```csharp
// In Program.cs or Startup.cs
public void ConfigureServices(IServiceCollection services)
{
    services.AddSingleton<IGCNotifyService, GCNotifyService>();
}
```

## Email Templates

### Create Bilingual Template

GC Notify templates support both English and French in a single template using conditional logic.

**Example: Application Confirmation**

**Subject**: Application Received / Demande reçue

**Body**:
```
((if english))
Dear ((first_name)) ((last_name)),

Thank you for submitting your application for Employment Insurance benefits.

**Application details:**
- Confirmation number: ((confirmation_number))
- Submission date: ((submission_date))
- Status: ((status))

**What happens next:**
We will review your application within 10 business days. You will receive an email when your application status changes.

If you have questions, contact us:
- Phone: 1-800-XXX-XXXX
- Email: benefits@servicecanada.gc.ca

Thank you,
Service Canada
((endif))

((if french))
Cher/Chère ((first_name)) ((last_name)),

Merci d'avoir soumis votre demande de prestations d'assurance-emploi.

**Détails de la demande :**
- Numéro de confirmation : ((confirmation_number))
- Date de soumission : ((submission_date))
- Statut : ((status))

**Prochaines étapes :**
Nous examinerons votre demande dans les 10 jours ouvrables. Vous recevrez un courriel lorsque le statut de votre demande changera.

Si vous avez des questions, contactez-nous :
- Téléphone : 1-800-XXX-XXXX
- Courriel : prestations@servicecanada.gc.ca

Merci,
Service Canada
((endif))
```

### Sending Email with Template

```csharp
public async Task SendApplicationConfirmationAsync(Application application, string language)
{
    var templateId = _configuration["GCNotify:Templates:ApplicationConfirmation"];

    var personalisation = new Dictionary<string, dynamic>
    {
        { "english", language == "en" ? "yes" : "" },
        { "french", language == "fr" ? "yes" : "" },
        { "first_name", application.FirstName },
        { "last_name", application.LastName },
        { "confirmation_number", application.ConfirmationNumber },
        { "submission_date", application.SubmissionDate.ToString("MMMM dd, yyyy") },
        { "status", "Under review" }
    };

    await _gcNotifyService.SendEmailAsync(
        templateId,
        application.Email,
        personalisation
    );
}
```

## SMS Templates

### Create SMS Template

SMS templates are limited to 160 characters per segment. Keep messages concise.

**Example: Status Update**

```
((if english))Service Canada: Your application ((confirmation_number)) status changed to ((status)). Check details at canada.ca/ei((endif))((if french))Service Canada : Le statut de votre demande ((confirmation_number)) a changé à ((status)). Consultez canada.ca/ae((endif))
```

### Sending SMS

```csharp
public async Task SendStatusUpdateSmsAsync(Application application, string status, string language)
{
    var templateId = _configuration["GCNotify:Templates:StatusUpdate"];

    var personalisation = new Dictionary<string, dynamic>
    {
        { "english", language == "en" ? "yes" : "" },
        { "french", language == "fr" ? "yes" : "" },
        { "confirmation_number", application.ConfirmationNumber },
        { "status", status }
    };

    // Normalize phone number to E.164 format
    var phoneNumber = NormalizePhoneNumber(application.PhoneNumber);

    await _gcNotifyService.SendSmsAsync(
        templateId,
        phoneNumber,
        personalisation
    );
}

private string NormalizePhoneNumber(string phoneNumber)
{
    // Remove formatting and add +1 for Canada
    var digits = new string(phoneNumber.Where(char.IsDigit).ToArray());
    return digits.Length == 10 ? $"+1{digits}" : $"+{digits}";
}
```

## Advanced Features

### Send with File Attachment

```csharp
public async Task SendEmailWithAttachmentAsync(
    string templateId,
    string emailAddress,
    Dictionary<string, dynamic> personalisation,
    byte[] fileContent,
    string fileName)
{
    // Upload file to GC Notify
    var fileResponse = await _client.PrepareUploadAsync(fileContent);

    // Add file link to personalisation
    personalisation.Add("link_to_file", fileResponse);

    // Send email
    await _client.SendEmailAsync(emailAddress, templateId, personalisation);
}
```

**Template must include**:
```
Download your document: ((link_to_file))
```

### Scheduled Sending

```csharp
public async Task ScheduleEmailAsync(
    string templateId,
    string emailAddress,
    Dictionary<string, dynamic> personalisation,
    DateTime scheduledFor)
{
    var response = await _client.SendEmailAsync(
        emailAddress,
        templateId,
        personalisation,
        scheduledFor: scheduledFor.ToString("yyyy-MM-dd HH:mm")
    );
}
```

### Get Notification Status

```csharp
public async Task<string> GetNotificationStatusAsync(string notificationId)
{
    var notification = await _client.GetNotificationByIdAsync(notificationId);
    return notification.status;
    // Possible values: "created", "sending", "delivered", "permanent-failure", "temporary-failure"
}
```

### Get All Notifications

```csharp
public async Task<List<Notification>> GetRecentNotificationsAsync()
{
    var response = await _client.GetNotificationsAsync(
        templateType: "email",
        status: "delivered",
        olderThan: null
    );

    return response.notifications;
}
```

## Error Handling

```csharp
public async Task<bool> TrySendEmailAsync(
    string templateId,
    string emailAddress,
    Dictionary<string, dynamic> personalisation)
{
    try
    {
        await _client.SendEmailAsync(emailAddress, templateId, personalisation);
        return true;
    }
    catch (NotifyClientException ex) when (ex.Message.Contains("ValidationError"))
    {
        _logger.LogWarning(
            "Invalid email address or template parameters: {Message}",
            ex.Message
        );
        return false;
    }
    catch (NotifyClientException ex) when (ex.Message.Contains("RateLimitError"))
    {
        _logger.LogWarning("GC Notify rate limit exceeded, will retry");
        await Task.Delay(TimeSpan.FromSeconds(5));
        return await TrySendEmailAsync(templateId, emailAddress, personalisation);
    }
    catch (NotifyClientException ex)
    {
        _logger.LogError(ex, "GC Notify API error");
        return false;
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Unexpected error sending notification");
        return false;
    }
}
```

## Rate Limits

- **Team and whitelist keys**: 250 emails/SMS per day
- **Live keys**: 250,000 emails/SMS per day per service
- **Rate limit**: 3,000 requests per minute

**Handle rate limits**:
```csharp
public async Task SendBulkEmailsAsync(List<EmailRequest> requests)
{
    var batchSize = 100;
    var delayBetweenBatches = TimeSpan.FromSeconds(2);

    for (int i = 0; i < requests.Count; i += batchSize)
    {
        var batch = requests.Skip(i).Take(batchSize);

        var tasks = batch.Select(request =>
            _gcNotifyService.SendEmailAsync(
                request.TemplateId,
                request.EmailAddress,
                request.Personalisation
            )
        );

        await Task.WhenAll(tasks);

        if (i + batchSize < requests.Count)
            await Task.Delay(delayBetweenBatches);
    }
}
```

## Template Best Practices

### 1. Use Personalization Variables

```
Dear ((first_name)),

Your application ((confirmation_number)) has been received.
```

### 2. Keep Templates Concise

- Email: Aim for < 200 words
- SMS: < 160 characters (1 segment)

### 3. Include Clear Actions

```
**Next steps:**
1. Review your application details
2. Upload supporting documents
3. Check your email for updates

[Button: View Application](((application_url)))
```

### 4. Test Bilingual Rendering

Always test both English and French versions:
```csharp
[Fact]
public async Task Template_RendersCorrectly_InBothLanguages()
{
    var personalisation = new Dictionary<string, dynamic>
    {
        { "first_name", "Test" }
    };

    // Test English
    personalisation["english"] = "yes";
    personalisation["french"] = "";
    var emailEn = await SendTestEmailAsync(personalisation);
    emailEn.Should().Contain("Dear Test");

    // Test French
    personalisation["english"] = "";
    personalisation["french"] = "yes";
    var emailFr = await SendTestEmailAsync(personalisation);
    emailFr.Should().Contain("Cher/Chère Test");
}
```

### 5. Accessibility

- Use headings properly
- Provide alt text for images
- Ensure color contrast
- Test with screen readers

GC Notify templates are automatically accessible, but custom content should follow WCAG 2.1 AA.

## Common Use Cases

### Application Confirmation

```csharp
public async Task SendApplicationConfirmationAsync(Application application)
{
    var templateId = _configuration["GCNotify:Templates:ApplicationConfirmation"];
    var language = application.PreferredLanguage; // "en" or "fr"

    var personalisation = new Dictionary<string, dynamic>
    {
        { "english", language == "en" ? "yes" : "" },
        { "french", language == "fr" ? "yes" : "" },
        { "first_name", application.FirstName },
        { "last_name", application.LastName },
        { "confirmation_number", application.ConfirmationNumber },
        { "submission_date", application.SubmissionDate.ToString("MMMM dd, yyyy") }
    };

    await _gcNotifyService.SendEmailAsync(templateId, application.Email, personalisation);
}
```

### Password Reset

```csharp
public async Task SendPasswordResetAsync(string email, string resetToken, string language)
{
    var templateId = _configuration["GCNotify:Templates:PasswordReset"];
    var resetUrl = $"https://yourdomain.gc.ca/reset-password?token={resetToken}";

    var personalisation = new Dictionary<string, dynamic>
    {
        { "english", language == "en" ? "yes" : "" },
        { "french", language == "fr" ? "yes" : "" },
        { "reset_url", resetUrl },
        { "expiry_time", "24 hours / 24 heures" }
    };

    await _gcNotifyService.SendEmailAsync(templateId, email, personalisation);
}
```

### Two-Factor Authentication

```csharp
public async Task Send2FACodeAsync(string phoneNumber, string code, string language)
{
    var templateId = _configuration["GCNotify:Templates:TwoFactorAuth"];

    var personalisation = new Dictionary<string, dynamic>
    {
        { "english", language == "en" ? "yes" : "" },
        { "french", language == "fr" ? "yes" : "" },
        { "code", code }
    };

    await _gcNotifyService.SendSmsAsync(templateId, phoneNumber, personalisation);
}
```

## Testing

### Unit Tests with Mock

```csharp
public class NotificationServiceTests
{
    private readonly Mock<IGCNotifyService> _mockGCNotify;
    private readonly ApplicationService _sut;

    public NotificationServiceTests()
    {
        _mockGCNotify = new Mock<IGCNotifyService>();
        _sut = new ApplicationService(_mockGCNotify.Object);
    }

    [Fact]
    public async Task SubmitApplication_SendsConfirmationEmail()
    {
        // Arrange
        var application = new Application
        {
            Email = "test@example.com",
            FirstName = "John",
            PreferredLanguage = "en"
        };

        _mockGCNotify
            .Setup(n => n.SendEmailAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<Dictionary<string, dynamic>>()))
            .ReturnsAsync("notification-id-123");

        // Act
        await _sut.SubmitApplicationAsync(application);

        // Assert
        _mockGCNotify.Verify(
            n => n.SendEmailAsync(
                It.IsAny<string>(),
                application.Email,
                It.Is<Dictionary<string, dynamic>>(d => d["first_name"].ToString() == "John")),
            Times.Once
        );
    }
}
```

### Integration Tests

```csharp
[Fact]
public async Task SendEmail_ToWhitelistedAddress_Succeeds()
{
    // Note: Use Team and whitelist key for testing
    var service = new GCNotifyService(_configuration, _logger);
    var templateId = _configuration["GCNotify:Templates:Test"];

    var personalisation = new Dictionary<string, dynamic>
    {
        { "test_value", "Integration test" }
    };

    // Use whitelisted test email
    var notificationId = await service.SendEmailAsync(
        templateId,
        "test@example.com", // Must be in whitelist
        personalisation
    );

    notificationId.Should().NotBeNullOrEmpty();
}
```

## Security Considerations

1. **Never log API keys**:
```csharp
// ❌ BAD
_logger.LogInformation($"Using API key: {apiKey}");

// ✅ GOOD
_logger.LogInformation("GC Notify service initialized");
```

2. **Validate email addresses**:
```csharp
private bool IsValidEmail(string email)
{
    return MailAddress.TryCreate(email, out _);
}
```

3. **Sanitize user input**:
```csharp
private Dictionary<string, dynamic> SanitizePersonalisation(Dictionary<string, dynamic> personalisation)
{
    return personalisation.ToDictionary(
        kvp => kvp.Key,
        kvp => kvp.Value is string str ? HttpUtility.HtmlEncode(str) : kvp.Value
    );
}
```

4. **Store API keys securely**:
- Use Azure Key Vault in production
- Use user secrets in development
- Never commit to source control

## Monitoring

```csharp
public class GCNotifyMetrics
{
    private readonly ILogger<GCNotifyMetrics> _logger;

    public async Task LogNotificationMetricsAsync()
    {
        var notifications = await _client.GetNotificationsAsync();

        var delivered = notifications.notifications.Count(n => n.status == "delivered");
        var failed = notifications.notifications.Count(n => n.status == "permanent-failure");
        var total = notifications.notifications.Count;

        _logger.LogInformation(
            "GC Notify metrics: Total={Total}, Delivered={Delivered}, Failed={Failed}, DeliveryRate={Rate:P}",
            total,
            delivered,
            failed,
            (double)delivered / total
        );
    }
}
```

## Resources

- **GC Notify Documentation**: https://documentation.notification.canada.ca/
- **GC Notify Dashboard**: https://notification.canada.ca/
- **.NET Client**: https://github.com/cds-snc/notification-net-client
- **Support**: notification-notification@cds-snc.ca

## Version Information

This guidance is current for:
- GC Notify API v2
- .NET Client v7.0.0+
- Notify.Client NuGet package

Last updated: 2025-02-11
