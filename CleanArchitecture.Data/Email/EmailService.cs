using CleanArchitecture.Application.Contracts.Infrastucture;
using CleanArchitecture.Application.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace CleanArchitecture.Infrastructure.Email
{
    public class EmailService(IOptions<EmailSettings> emailSettings, ILogger<EmailService> logger) : IEmailService
    {
        public EmailSettings _emailSettings { get; } = emailSettings.Value;
        public ILogger<EmailService> _logger { get; } = logger;

        public async Task<bool> SendEmail(Application.Models.Email email)
        {
            var client = new SendGridClient(_emailSettings.ApiKey);

            var subject = email.Subject;
            var to = new EmailAddress(email.To);
            var body = email.Body;

            var from = new EmailAddress
            {
                Email = _emailSettings.FromAddress,
                Name = _emailSettings.FromName
            };

            var sendGridMessage = MailHelper.CreateSingleEmail(from, to, subject, body, body);

            var response = await client.SendEmailAsync(sendGridMessage);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Couldn't send the email, errors were found");
                return false;
            }

            return true;
        }
    }
}
