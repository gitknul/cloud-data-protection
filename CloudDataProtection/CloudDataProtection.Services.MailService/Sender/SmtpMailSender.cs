using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using CloudDataProtection.Services.MailService.Options;
using Microsoft.Extensions.Options;
using SendGrid;

namespace CloudDataProtection.Services.MailService.Sender
{
    public class SmtpMailSender : MailSenderBase, IMailSender
    {
        private readonly SmtpOptions _smtpOptions;

        public SmtpMailSender(IOptions<SmtpOptions> smtpOptions, IOptions<MailOptions> mailOptions) : base(mailOptions)
        {
            _smtpOptions = smtpOptions.Value;
        }

        public async Task Send(string recipient, string subject, string body)
        {
            MailMessage message = new MailMessage(MailOptions.Sender, recipient, subject, StripHtml(body));

            message.IsBodyHtml = false;

            message.AlternateViews.Add(CreateHtmlAlternateView(body));
            
            using (SmtpClient client = new SmtpClient(_smtpOptions.Host, _smtpOptions.Port))
            {
                // Only use credentials if they are supplied
                if (!string.IsNullOrWhiteSpace(_smtpOptions.User))
                {
                    client.Credentials = new NetworkCredential(_smtpOptions.User, _smtpOptions.Password);
                }
                
                await client.SendMailAsync(message);
            }
        }

        private static AlternateView CreateHtmlAlternateView(string body)
        {
            return AlternateView.CreateAlternateViewFromString(body, Encoding.UTF8, MimeType.Html);
        }
    }
}