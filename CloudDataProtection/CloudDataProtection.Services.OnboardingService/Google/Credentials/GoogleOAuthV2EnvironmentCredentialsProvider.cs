﻿using CloudDataProtection.Core.Environment;

namespace CloudDataProtection.Services.Onboarding.Google.Credentials
{
    public class GoogleOAuthV2EnvironmentCredentialsProvider : IGoogleOAuthV2CredentialsProvider
    {
        private static readonly string ClientIdEnvironmentKey = "CDP_ONBOARDING_GOOGLE_OAUTH2_CLIENT_ID";
        private static readonly string ClientSecretEnvironmentKey = "CDP_ONBOARDING_GOOGLE_OAUTH2_CLIENT_SECRET";

        public string ClientId => EnvironmentVariableHelper.GetEnvironmentVariable(ClientIdEnvironmentKey);
        public string ClientSecret => EnvironmentVariableHelper.GetEnvironmentVariable(ClientSecretEnvironmentKey);
    }
}