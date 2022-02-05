# Cloud Data Protection mail service

## Environment variables

### Summary

| Name                        | Value                   | Used by      | Required |
|-----------------------------|-------------------------|--------------|----------|
| CDP_SG_API_KEY              | Sendgrid API key        | Mail service | Yes      |
| CDP_SG_SENDER               | Sendgrid sender address | Mail service | Yes      |
| CDP_PAPERTRAIL_URL          | Papertrail endpoint     | All services | Yes      |
| CDP_PAPERTRAIL_ACCESS_TOKEN | Papertrail access token | All services | Yes      |

### Mail service

#### SendGrid

SendGrid is used to send mails. Create a API key and verify a sender address.
