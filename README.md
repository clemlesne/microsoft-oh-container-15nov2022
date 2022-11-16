# Containers 2.0 Openhack



<!--
Guidelines on README format: https://review.docs.microsoft.com/help/onboard/admin/samples/concepts/readme-template?branch=master

Guidance on onboarding samples to docs.microsoft.com/samples: https://review.docs.microsoft.com/help/onboard/admin/samples/process/onboarding?branch=master

Taxonomies for products and languages: https://review.docs.microsoft.com/new-hope/information-architecture/metadata/taxonomies?branch=master
-->

This repo houses the source code and dockerfiles for the Containers OpenHack event.

The application used for this event is a heavily modified and recreated version of the original [My Driving application](https://github.com/Azure-Samples/MyDriving).

## Development

For some of these steps, make sure you are connected to the Azure portal using:

```bash
# Login to Azure
az login
az account set --subscription abfe0518-15b0-4905-95f9-8a7002dbfec1
```

### Build

```bash
docker-compose -f cicd/docker-compose.yaml build --build-arg IMAGE_CREATE_DATE="`date -u +"%Y-%m-%dT%H:%M:%SZ"`" --build-arg IMAGE_SOURCE_REVISION="`git rev-parse HEAD`" --build-arg IMAGE_VERSION="`git describe --tags --abbrev=0`"
```

### Run locally

```bash
# Deploy
docker-compose -f cicd/docker-compose.yaml up -d
# See logs
docker-compose -f cicd/docker-compose.yaml logs -f
```

### Push to Azure Container Registry (ACR)

```bash
# Login local docker environment to the remote container registry
az acr login -n registrykar3457
# Push
docker-compose -f cicd/docker-compose.yaml push
```

### Setup automation

```bash
# Create role and assign permissions
az ad sp create-for-rbac --name "github-actions" --role contributor --scopes /subscriptions/abfe0518-15b0-4905-95f9-8a7002dbfec1 --sdk-auth
# Store the output to a GitHub Actions secret variable named "AZURE_CREDS"
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
