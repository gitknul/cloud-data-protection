# Cloud Data Protection gateway

## Environment variables

### Summary

| Name                            | Value                   | Used by       | Required |
|---------------------------------|-------------------------|---------------|----------|
| CDP_GATEWAY_BACKUP_DEMO_API_KEY | Backup demo API key     | Gateway       | Yes      |
| CDP_PAPERTRAIL_URL              | Papertrail endpoint     | All services  | Yes      |
| CDP_PAPERTRAIL_ACCESS_TOKEN     | Papertrail access token | All services  | Yes      |

### Gateway

#### Function authentication

Generate a API key and store it in the `CDP_GATEWAY_BACKUP_DEMO_API_KEY` variable. It is set by default in `launchSettings.json`.
