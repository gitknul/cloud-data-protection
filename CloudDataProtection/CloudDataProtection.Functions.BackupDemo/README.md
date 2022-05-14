# Cloud Data Protection backup demo serverless functions

## Environment variables

### Summary

| Name                                    | Value                            | Used by                         | Required |
|-----------------------------------------|----------------------------------|---------------------------------|----------|
| CDP_BACKUP_DEMO_API_KEY                 | SHA-512 hash of API key          | Backup demo serverless function | Yes      |
| CDP_BACKUP_DEMO_BLOB_AES_KEY            | AES 256 key                      | Backup demo serverless function | Yes      |
| CDP_BACKUP_DEMO_BLOB_AES_IV             | AES 256 iv                       | Backup demo serverless function | Yes      |
| CDP_BACKUP_DEMO_AWS_KEY                 | AWS S3 key                       | Backup demo serverless function | No *     |
| CDP_BACKUP_DEMO_AWS_SECRET              | AWS S3 secret                    | Backup demo serverless function | No *     |
| CDP_BACKUP_DEMO_BLOB_STORAGE_CONNECTION | Azure Blob Storage connection    | Backup demo serverless function | No *     |
| CDP_BACKUP_DEMO_GCS_PROJECT_ID          | Google Cloud Storage project id  | Backup demo serverless function | No *     |
| CDP_BACKUP_DEMO_GCS_JSON_FILE           | Google Cloud Storage credentials | Backup demo serverless function | No *     |
| CDP_BACKUP_DEMO_SETTINGS_FILE           | Path to appsettings.json file    | Backup demo serverless function | No **    |
| AZURE_FUNCTIONS_ENVIRONMENT             | Hosting environment              | Backup demo serverless function | Yes      |

&ast; Only required if the destination is enabled in the app settings\
&ast;&ast; Only required if you want to use a custom path to read the app settings from

### Backup demo

#### Destinations

Backup destinations can be enabled and disabled by modifying the `appsettings.json` file for the corresponding environment.

#### MongoDB

The MongoDB-database in embedded in the Docker Compose configuration. The connection string and database only need to be stored as environment variables.

| Key                                | Value                                                                          |
|------------------------------------|--------------------------------------------------------------------------------|
| CDP_BACKUP_DEMO_MONGODB_CONNECTION | `mongodb://root:mongodevelopment@localhost:27017/?retryWrites=true&w=majority` |
| CDP_BACKUP_DEMO_MONGODB_DATABASE   | `backupdemodb`                                                                 |

#### Azure Blob Storage

Create an Azure account. Create a blob storage account and retrieve the connection string.

#### Amazon S3

Create a AWS account with a IAM user (see [here](https://docs.aws.amazon.com/sdk-for-net/latest/developer-guide/quick-start-s3-1-cross.html#s3-1-cross-setup)). Retrieve the AWS key and secret ID.

#### Google Cloud Storage

Create a Google account. In the Google Cloud Console, create a new project and store the identifier. Create a service account with a `Storage Admin` role for the project you just created and add a new key to that service account. Download the JSON file, store it and set the path in an environment variable.
