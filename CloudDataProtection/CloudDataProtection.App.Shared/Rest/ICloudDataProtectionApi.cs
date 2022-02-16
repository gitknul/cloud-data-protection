using System;
using Refit;

namespace CloudDataProtection.App.Shared.Rest
{
    public interface ICloudDataProtectionApi
    {
        [Post("/Authentication/Authenticate")]
        IObservable<AuthenticateOutput> Authenticate(AuthenticateInput input);

        [Get("/Search")]
        IObservable<SearchOutput> Search([AliasAs("text")] string text);
    }
}
