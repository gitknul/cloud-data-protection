# Cloud Data Protection mail service

## Environment variables

### Summary

| Name                        | Value                   | Used by      | Required |
|-----------------------------|-------------------------|--------------|----------|
| CDP_PAPERTRAIL_URL          | Papertrail endpoint     | All services | Yes      |
| CDP_PAPERTRAIL_ACCESS_TOKEN | Papertrail access token | All services | Yes      |

### Mail service

#### SendGrid

SendGrid is used to send mails. Create a API key and verify a sender address.

## Configuration

Configuration can be set using app settings files or environment variables. In the examples below app settings are used.

### Sections

#### Mail

Mails can be sent using different methods:

* SMTP `smtp`
* Sendgrid `sendgrid`

```json
  "Mail": {
    "Protocol": "Smtp",
    "Sender": "test@example.com"
  }
```

#### SMTP

The SMTP section must be configured if mails are sent using SMTP. See [SmtpOptions.cs](SMTP/Credentials/SmtpOptions.cs) for all available variables.

```json
  "Smtp": {
    "Host": "127.0.0.1",
    "Port": 1025
  }
```

#### Sendgrid

The Sendgrid section must be configured if mails are sent using Sendgrid.

```json
  "SendGrid": {
    "ApiKey": "SG.xxxx"
  }
```