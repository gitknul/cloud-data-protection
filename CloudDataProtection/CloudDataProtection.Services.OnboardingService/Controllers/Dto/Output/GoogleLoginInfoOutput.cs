using System.Collections.Generic;

namespace CloudDataProtection.Services.Onboarding.Controllers.Dto.Output
{
    public class GoogleLoginInfoOutput
    {
        public string State { get; set; }
        
        public string ClientId { get; set; }
        
        public string RedirectUri { get; set; }
        
        public IEnumerable<string> Scopes { get; set; }
    }
}