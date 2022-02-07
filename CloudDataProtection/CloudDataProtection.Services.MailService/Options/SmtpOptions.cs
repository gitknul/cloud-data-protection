using System.ComponentModel.DataAnnotations;

namespace CloudDataProtection.Services.MailService.Options
{
    public class SmtpOptions
    {
        [Required(AllowEmptyStrings = false)]
        public string Host { get; set; }
        
        [Required(AllowEmptyStrings = false)]
        public int Port { get; set; }

        public string User { get; set; }
        
        public string Password { get; set; }
    }
}