# Cloud Data Protection backend

## Dependencies

Run the following script to launch all the dependencies for the backend.

```bash
docker compose -f docker-compose.dev.yml up -d
```

## Generating encryption and API keys

Run the `CloudDataProtection.CI.KeyGenerator` project to generate AES256 and API keys:

```bash
dotnet run --project CloudDataProtection.CI.KeyGenerator
```

## Services

- [Gateway](CloudDataProtection.Gateway/README.md)
- [Mail](CloudDataProtection.Services.MailService/README.md)
- [Backup configuration](CloudDataProtection.Services.BackupConfigurationService/README.md)
- [Onboarding](CloudDataProtection.Services.OnboardingService/README.md)

## Serverless services

* [Backup demo](CloudDataProtection.Functions.BackupDemo/README.md)

## Logging in

A client account can be created by registering a new account. It is not possible yet to change the password of the default (employee) user.
