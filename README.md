# Containers 2.0 Openhack



<!--
Guidelines on README format: https://review.docs.microsoft.com/help/onboard/admin/samples/concepts/readme-template?branch=master

Guidance on onboarding samples to docs.microsoft.com/samples: https://review.docs.microsoft.com/help/onboard/admin/samples/process/onboarding?branch=master

Taxonomies for products and languages: https://review.docs.microsoft.com/new-hope/information-architecture/metadata/taxonomies?branch=master
-->

This repo houses the source code and dockerfiles for the Containers OpenHack event.

The application used for this event is a heavily modified and recreated version of the original [My Driving application](https://github.com/Azure-Samples/MyDriving).

## Development

### Build

```bash
# poi
docker build -t poi:latest ./src/poi
# trips-api
docker build -t trips-api:latest ./src/trips
# trips-viewer
docker build -t trips-viewer:latest ./src/tripviewer
# users-api
docker build -t users-api:latest ./src/user-java
# profiles-api
docker build -t profiles-api:latest ./src/userprofile
```

### Run locally

```bash
# Pull the MSSQL database image locally (see: https://mcr.microsoft.com/en-us/product/mssql/server/about)
docker pull mcr.microsoft.com/mssql/server:2017-latest
# Run the database
docker run --rm -ti -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Password1234_" -p 1433:1433 mcr.microsoft.com/azure-sql-edge:latest
# Run poi
docker run -e "SQL_USER=SA" -e "SQL_PASSWORD=Password1234_" -e "SQL_SERVER=localhost:1433" -p 8001:80 poi:latest
# Run trips-api
docker run -e "SQL_USER=SA" -e "SQL_PASSWORD=Password1234_" -e "SQL_SERVER=localhost:1433" -e "DEBUG_LOGGING=true" -p 8002:80 trips-api:latest
# Run trips-viewer
docker run -e "SQL_USER=SA" -e "SQL_PASSWORD=Password1234_" -e "SQL_SERVER=localhost:1433" -p 8003:80 trips-viewer:latest
# Run users-api
docker run -e "SQL_USER=SA" -e "SQL_PASSWORD=Password1234_" -e "SQL_SERVER=localhost:1433" -p 8004:80 users-api:latest
# Run profiles-api
docker run -e "SQL_USER=SA" -e "SQL_PASSWORD=Password1234_" -e "SQL_SERVER=localhost:1433" -p 8005:80 profiles-api:latest
```

### Push to Azure Container Registry (ACR)

```bash
# Login to Azure
az login
az account set --subscription abfe0518-15b0-4905-95f9-8a7002dbfec1
# Login local docker environment to the remote container registry
az acr login -n registrykar3457
# Push poi
docker tag poi:latest registrykar3457.azurecr.io/poi:latest
# Push trips-api
docker tag trips-api:latest registrykar3457.azurecr.io/trips-api:latest
# Push trips-viewer
docker tag trips-viewer:latest registrykar3457.azurecr.io/trips-viewer:latest
# Push users-api
docker tag users-api:latest registrykar3457.azurecr.io/users-api:latest
# Push profiles-api
docker tag profiles-api:latest registrykar3457.azurecr.io/profiles-api:latest
```

## Contents

| File/folder       | Description                                |
|-------------------|--------------------------------------------|
| `.devcontainer`   | VS Code [development container](https://code.visualstudio.com/docs/remote/containers) with useful utils (Azure CLI, Kubectl, Helm, etc.)   |
| `dockerfiles`     | Dockerfiles for source code                |
| `src`             | Sample source code for POI, Trips, User (Java), UserProfile (Node.JS), and TripViewer                     |
| `.gitignore`      | Define what to ignore at commit time.      |
| `CODE_OF_CONDUCT.md` | Code of conduct.                        |
| `LICENSE`         | The license for the sample.                |

## Contributing

This project welcomes contributions and suggestions.  Most contributions require you to agree to a
Contributor License Agreement (CLA) declaring that you have the right to, and actually do, grant us
the rights to use your contribution. For details, visit https://cla.opensource.microsoft.com.

When you submit a pull request, a CLA bot will automatically determine whether you need to provide
a CLA and decorate the PR appropriately (e.g., status check, comment). Simply follow the instructions
provided by the bot. You will only need to do this once across all repos using our CLA.

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/).
For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or
contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.
