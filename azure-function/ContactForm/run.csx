#r "Newtonsoft.Json"

using System.Net;
using System.Net.Mail;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

public static async Task<IActionResult> Run(HttpRequest req, ILogger log)
{
    log.LogInformation("Contact form submission received");

    // Read form data
    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
    var formData = System.Web.HttpUtility.ParseQueryString(requestBody);

    string fromEmail = formData["fromEmail"];
    string message = formData["message"];

    // Validate required fields
    if (string.IsNullOrWhiteSpace(fromEmail) || string.IsNullOrWhiteSpace(message))
    {
        log.LogWarning("Missing required fields");
        return new BadRequestObjectResult("Missing required fields: fromEmail and message");
    }

    // Validate email format
    try
    {
        var addr = new System.Net.Mail.MailAddress(fromEmail);
        if (addr.Address != fromEmail)
        {
            return new BadRequestObjectResult("Invalid email address");
        }
    }
    catch
    {
        return new BadRequestObjectResult("Invalid email address");
    }

    // Get configuration from app settings
    string smtpHost = Environment.GetEnvironmentVariable("SMTP_HOST");
    string smtpPortStr = Environment.GetEnvironmentVariable("SMTP_PORT");
    string smtpUser = Environment.GetEnvironmentVariable("SMTP_USER");
    string smtpPass = Environment.GetEnvironmentVariable("SMTP_PASS");
    string toEmail = Environment.GetEnvironmentVariable("TO_EMAIL");
    string smtpEnableSslStr = Environment.GetEnvironmentVariable("SMTP_ENABLE_SSL");

    // Validate configuration
    if (string.IsNullOrWhiteSpace(smtpHost) ||
        string.IsNullOrWhiteSpace(smtpPortStr) ||
        string.IsNullOrWhiteSpace(toEmail))
    {
        log.LogError("SMTP configuration is incomplete");
        return new StatusCodeResult(500);
    }

    int smtpPort = int.Parse(smtpPortStr);
    bool smtpEnableSsl = bool.Parse(smtpEnableSslStr ?? "true");

    try
    {
        // Create email message
        var mailMessage = new MailMessage
        {
            From = new MailAddress(smtpUser ?? fromEmail),
            Subject = "Contact Form Submission - robjohnston.github.io",
            Body = $"From: {fromEmail}\n\nMessage:\n{message}",
            IsBodyHtml = false
        };
        mailMessage.To.Add(toEmail);
        mailMessage.ReplyToList.Add(fromEmail);

        // Configure SMTP client
        using (var smtpClient = new SmtpClient(smtpHost, smtpPort))
        {
            smtpClient.EnableSsl = smtpEnableSsl;

            if (!string.IsNullOrWhiteSpace(smtpUser) && !string.IsNullOrWhiteSpace(smtpPass))
            {
                smtpClient.Credentials = new NetworkCredential(smtpUser, smtpPass);
            }

            // Send email
            await smtpClient.SendMailAsync(mailMessage);
        }

        log.LogInformation($"Email sent successfully from {fromEmail}");
        return new OkObjectResult("Thank you! Your message has been sent successfully.");
    }
    catch (Exception ex)
    {
        log.LogError($"Failed to send email: {ex.Message}");
        return new StatusCodeResult(500);
    }
}
