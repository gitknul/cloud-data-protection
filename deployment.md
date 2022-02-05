# Services

## Kubernetes

| Name                  | Host/port                 | External  | 
| -------------         | -----                     | ---       | 
| Frontend              | loadbalancer:80           | ✅        | 
| Gateway               | loadbalancer:5001         | ✅        |
| BackupConfiguration   | backup-config-cluster-ip  | ❌        | 
| Onboarding            | onboarding-cluster-ip     | ❌        |
| Mail                  | none                      | ❌        |

# Secrets

## Kubernetes

| Name in code                                      | Name in k8s                                   |
| -------------                                     | -----                                         |
| CDP_ONBOARDING_GOOGLE_OAUTH2_CLIENT_ID            | cdp-onboarding-google-oauth2-client-id        |
| CDP_ONBOARDING_GOOGLE_OAUTH2_CLIENT_SECRET        | cdp-onboarding-google-oauth2-client-secret    |
| CDP_SG_API_KEY                                    | cdp-sendgrid                                  |
| CDP_SG_SENDER                                     | cdp-sendgrid-sender                           |
| CDP_BACKUP_DEMO_FUNCTIONS_KEY                     | cdp-backup-demo-api-key                       |
| CDP_PAPERTRAIL_ACCESS_TOKEN                       | cdp-papertrail-access-token                   |
| CDP_PAPERTRAIL_URL                                | cdp-papertrail-url                            |

## Functions

| Name                                    |
|-----------------------------------------|
| CDP_BACKUP_DEMO_API_KEY                 |
| CDP_BACKUP_DEMO_BLOB_AES_KEY            |
| CDP_BACKUP_DEMO_BLOB_AES_IV             |
| CDP_BACKUP_DEMO_AWS_KEY                 |
| CDP_BACKUP_DEMO_AWS_SECRET              |
| CDP_BACKUP_DEMO_BLOB_STORAGE_CONNECTION |
| CDP_BACKUP_DEMO_GCS_PROJECT_ID          |
| CDP_BACKUP_DEMO_GCS_JSON_FILE           |
| CDP_BACKUP_DEMO_SETTINGS_FILE           |