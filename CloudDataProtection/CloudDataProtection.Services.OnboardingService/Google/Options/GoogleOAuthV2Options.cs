// ReSharper disable UnusedAutoPropertyAccessor.Global

using System.Collections.Generic;

namespace CloudDataProtection.Services.Onboarding.Google.Options
{
    public class GoogleOAuthV2Options
    {
        public string Endpoint { get; set; }

        /// <summary>
        /// Users will be redirected to this path after they have authenticated with Google 
        /// </summary>
        public string RedirectUri { get; set; }
        
        public string GrantType { get; set; }
        
        public IEnumerable<string> Scopes { get; set; }
    }
}