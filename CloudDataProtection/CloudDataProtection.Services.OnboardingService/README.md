# Cloud Data Protection onboarding service

## Environment variables

### Summary

| Name                                       | Value                       | Used by                         | Required |
|--------------------------------------------|-----------------------------|---------------------------------|----------|
| CDP_ONBOARDING_GOOGLE_OAUTH2_CLIENT_ID     | Google OAuth2 Client ID     | Onboarding service              | Yes      |
| CDP_ONBOARDING_GOOGLE_OAUTH2_CLIENT_SECRET | Google OAuth2 Client secret | Onboarding service              | Yes      |
| CDP_PAPERTRAIL_URL                         | Papertrail endpoint         | Backup demo serverless function | Yes      |
| CDP_PAPERTRAIL_ACCESS_TOKEN                | Papertrail access token     | Backup demo serverless function | Yes      |

### Onboarding service

#### Google Oauth

1. Open the [OAuth consent screen page](https://console.cloud.google.com/apis/credentials/consent) and create a consent screen with the following configuration:

| Name               | Value    |
|--------------------|----------|
| User type          | External |
| Authorized domains | None     |

2. Enable the scopes `https://www.googleapis.com/auth/drive`, `https://www.googleapis.com/auth/drive.readonly`, `https://www.googleapis.com/auth/userinfo.email`, `https://www.googleapis.com/auth/userinfo.profile` and `openid`.

3. Add the email addresses you want to use for testing.

4. Create a [OAuth client ID](https://console.cloud.google.com/apis/credentials/oauthclient) with the following configuration:

```
Type: Web application
Authorized JavaScript origins: https://localhost:5001
Authorized redirect URIs: https://localhost:5001/Callback/GoogleLogin
```
