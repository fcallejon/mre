# MuRe Scoring

## How to run it (assuming on a Windows)

### Pre-requisites

1. Install OpenFaas: [here](https://docs.openfaas.com/deployment/kubernetes/#c-deploy-using-kubectl-and-plain-yaml-for-development-only)
2. Install Docker
3. Get a Docker account and login locally

### Run it

1. Map OpenFaas to localhost port: `kubectl port-forward svc/gateway -n openfaas 31112:8080`
2. Run deploy.ps1 [docker-login-id] [openfaas-gateway] "[random-org-apikey]"
3. Check score function at [OpenFaas UI](http://localhost:31112/ui/)
	- sample request body {"documents": ["Munich", "RE", "Scoring", "Test"] }
4. Go to the [MuReUI](http://localhost:31112/function/mureui)
