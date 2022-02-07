using System;
using System.ComponentModel.DataAnnotations;

namespace CloudDataProtection.Services.MailService.Options
{
    public class MailOptions
    {
        [Required]
        public string Protocol { get; set; }
        
        public MailProtocol MailProtocol => Enum.Parse<MailProtocol>(Protocol, true);
        
        [Required]
        public string Sender { get; set; }
    }
}