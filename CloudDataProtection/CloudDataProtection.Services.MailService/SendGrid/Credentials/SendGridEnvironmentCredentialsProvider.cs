using CloudDataProtection.Core.Environment;

namespace CloudDataProtection.Services.MailService.SendGrid.Credentials
{
    public class SendGridEnvironmentCredentialsProvider : ISendGridCredentialsProvider
    {
        public string ApiKey => EnvironmentVariableHelper.GetEnvironmentVariable("CDP_SG_API_KEY");
        public string SenderEmail => EnvironmentVariableHelper.GetEnvironmentVariable("CDP_SG_SENDER");
    }
}