using System.Threading.Tasks;
using CloudDataProtection.Services.MailService.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace CloudDataProtection.Services.MailService.Sender
{
    public class SendGridMailSender : MailSenderBase, IMailSender
    {
        private readonly SendGridOptions _sendGridOptions;
        private readonly ILogger<SendGridMailSender> _logger;
        
        public SendGridMailSender(IOptions<SendGridOptions> credentialsProvider, IOptions<MailOptions> mailOptions, ILogger<SendGridMailSender> logger) : base(mailOptions)
        {
            _sendGridOptions = credentialsProvider.Value;
            _logger = logger;
        }
        
        public async Task Send(string recipient, string subject, string body)
        {
            SendGridClient client = new SendGridClient(_sendGridOptions.ApiKey);

            SendGridMessage message = CreateMessage(recipient, subject, body, MailOptions.Sender);

            Response response = await client.SendEmailAsync(message);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("An error occured while sending a request to SendGrid: {Status}, {Body}", response.StatusCode, await response.Body.ReadAsStringAsync());
            }
        }

        private static SendGridMessage CreateMessage(string recipient, string subject, string body, string sender)
        {
            return MailHelper.CreateSingleEmail(
                new EmailAddress(sender, "Cloud Data Protection (development)"), 
                new EmailAddress(recipient),
                subject, StripHtml(body), body);
        }
    }
}