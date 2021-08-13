# Cloud Data Protection

Continued development of my school project that I made in the sixth semester for Fontys Hogeschool ICT, [Cloud Data Protection](https://github.com/olivierbouchomsfontys/cloud-data-protection).

Branches:

* Test: test environment
* Develop: development branch

# How to run locally

## SendGrid

SendGrid is used to send mails in the development environment. Create a user account and API key on SendGrid and store the API key in an environment variable.

Unix:
`export CDP_DEV_SENDGRID={your api key goes here}`

Windows:
`setx CDP_DEV_SENDGRID {your api key goes here}`

You also need to verify your email address to send mails, see https://sendgrid.com/docs/ui/sending-email/sender-verification/. Then it should be set as an environment variable.

Unix:
`export CDP_DEV_SENDGRID_SENDER={your email goes here}`

Windows:
`setx CDP_DEV_SENDGRID_SENDER {your email goes here}`

## Google

Create a Client ID and Client secret in the Google Cloud Console. Store the Client ID and Client Secret in an environment variable.

Unix:  
`export CDP_DEV_ONBOARDING_GOOGLE_OAUTH2_CLIENT_ID={your client id goes here}`  
`export CDP_DEV_ONBOARDING_GOOGLE_OAUTH2_CLIENT_SECRET={your client secret goes here}`

Windows:  
`setx CDP_DEV_ONBOARDING_GOOGLE_OAUTH2_CLIENT_ID {your client id goes here}`  
`setx CDP_DEV_ONBOARDING_GOOGLE_OAUTH2_CLIENT_SECRET {your client secret goes here}`

## Backup demo encryption

We need an AES256 encryption key and Iv. Generate it and store it in environment variables. The used key size is 256, while the used block size is 128.

Unix:  
`export CDP_DEMO_BLOB_AES_KEY={your AES key goes here}`  
`export CDP_DEMO_BLOB_AES_IV={your AES Iv goes here}`

Windows:  
`setx CDP_DEMO_BLOB_AES_KEY {your AES key goes here}`  
`setx CDP_DEMO_BLOB_AES_IV {your AES Iv goes here}`

## Backup demo destinations

Backup destinations are enabled by default in the backup demo. You can disable them by setting the following environment variables to `1` or `TRUE`:

Unix:  
`export CDP_DEMO_AWS_DISABLE=1`  
`export CDP_DEMO_BLOB_DISABLE=1`  
`export CDP_DEMO_GCS_DISABLE=1`

Windows:  
`export CDP_DEMO_AWS_DISABLE 1`  
`export CDP_DEMO_BLOB_DISABLE 1`  
`export CDP_DEMO_GCS_DISABLE 1`

## Azure Blob Storage

Create an Azure account. Create a blob storage account and retrieve the connection string. Store it in an environment variable.

Unix:
`export CDP_DEMO_BLOB_CONNECTION={your connection string goes here}`

Windows:
`setx CDP_DEMO_BLOB_CONNECTION {your connection string goes here}`

## Amazon S3

Create a AWS account with a IAM user (see [here](https://docs.aws.amazon.com/sdk-for-net/latest/developer-guide/quick-start-s3-1-cross.html#s3-1-cross-setup)). Store the AWS key and secret ID in environment variables.

Unix:  
`export CDP_DEMO_AWS_KEY={your AWS key goes here}`  
`export CDP_DEMO_AWS_SECRET={your secret ID goes here}`

Windows:  
`setx CDP_DEMO_AWS_KEY {your AWS key goes here}`  
`setx CDP_DEMO_AWS_SECRET {your secret ID goes here}`

## Google Cloud Storage

Create a Google account. In the Google Cloud Console, create a new project and store the identifier. Create a service account with a `Storage Admin` role for the project you just created and add a new key to that service account. Download the JSON file, store it and set the path in an environment variable.

Unix:  
`export CDP_DEMO_GCS_PROJECT_ID={project_id goes here}`  
`export CDP_DEMO_GCS_JSON={json path goes hase}`

Windows:  
`setx CDP_DEMO_GCS_PROJECT_ID {project_id goes here}`  
`setx CDP_DEMO_GCS_JSON {json path goes hase}`

## Function authentication

Function authentication is done using an API-key. You can set it to any custom string you like.

Unix:  
`export CDP_BACKUP_DEMO_FUNCTIONS_KEY={your function key goes here}`  
`export CDP_DEMO_API_KEY={your function key goes here}`

Windows:  
`setx CDP_BACKUP_DEMO_FUNCTIONS_KEY {your function key goes here}`  
`setx CDP_DEMO_API_KEY {your function key goes here}`

## MongoDB

The MongoDB-database in embedded in the Docker Compose configuration. The connection string and database only need to be stored as environment variables.

Unix:  
`export CDP_DEMO_MONGO=mongodb://root:mongodevelopment@localhost:27017/?retryWrites=true&w=majority`  
`export CDP_DEMO_MONGO_DB=backupdemodb`

Windows:  
`setx CDP_DEMO_MONGO "mongodb://root:mongodevelopment@localhost:27017/?retryWrites=true&w=majority"`  
`setx CDP_DEMO_MONGO_DB backupdemodb`

## Docker compose

Run `dev.sh` located in the `CloudDataProtection` directory to launch the development dependencies.

## Logging in

A client account can be created by registering a new account. It is not possible yet to change the password of the default (employee) user.
