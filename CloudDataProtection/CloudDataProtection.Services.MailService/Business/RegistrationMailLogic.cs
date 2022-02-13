using System.Threading.Tasks;
using CloudDataProtection.Core.Messaging.Dto;
using CloudDataProtection.Services.MailService.Business.Base;
using CloudDataProtection.Services.MailService.Sender;

namespace CloudDataProtection.Services.MailService.Business
{
    public class RegistrationMailLogic : MailLogicBase
    {
        private readonly IMailSender _sender;

        public RegistrationMailLogic(IMailSender sender)
        {
            _sender = sender;
        }

        public async Task SendClientRegistered(string email)
        {
            string subject = "Welcome to Cloud Data Protection";
            string content = @"
<p>Dear Sir / Madam,<br><br>
    Congratulations! You just completed the first step to securing all your company data. Please log in to your account and complete your registration.<br><br>
    Yours sincerely,<br><br>
    Cloud Data Protection
  </p>";
            
            string body = ComposeBody(content);

            await _sender.Send(email, subject, body);
        }

        public async Task SendAdminRegistered(ResetPasswordMessage message)
        {
            string subject = "Welcome to Cloud Data Protection";
            string content = @$"
<p>Dear Sir / Madam,<br><br>
    Please click <a href='{message.Url}'>here</a> to set up your password.<br><br>
    If the link above doesn't work, please copy and paste the following link in your web browser: {message.Url}<br><br>
    This link will expire at {message.Expiration.ToString("F")}<br><br>
    Yours sincerely,<br><br>
    Cloud Data Protection
  </p>";
            
            string body = ComposeBody(content);

            await _sender.Send(message.Email, subject, body);
        }
    }
}