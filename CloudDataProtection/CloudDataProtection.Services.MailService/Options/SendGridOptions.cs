using System.ComponentModel.DataAnnotations;

namespace CloudDataProtection.Services.MailService.Options
{
    public class SendGridOptions
    {
        [Required]
        public string ApiKey { get; set; }
    }
}