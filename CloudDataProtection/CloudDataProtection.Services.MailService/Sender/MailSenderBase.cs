using System.Text.RegularExpressions;
using CloudDataProtection.Services.MailService.Options;
using Microsoft.Extensions.Options;

namespace CloudDataProtection.Services.MailService.Sender
{
    public abstract class MailSenderBase
    {
        protected readonly MailOptions MailOptions;

        protected MailSenderBase(IOptions<MailOptions> mailOptions)
        {
            MailOptions = mailOptions.Value;
        }

        protected static string StripHtml(string html)
        {
            return Regex.Replace(html, "<.*?>", string.Empty);
        }
    }
}