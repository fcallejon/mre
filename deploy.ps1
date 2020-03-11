param (
    [Parameter(Mandatory = $true, ValueFromPipeline = $true, ValueFromPipelineByPropertyName = $true)]
    [string] $docker_user_name,

    [Parameter(Mandatory = $true, ValueFromPipeline = $true, ValueFromPipelineByPropertyName = $true)]
    [string] $openfaas_gateway,

    [Parameter(Mandatory = $true, ValueFromPipeline = $true, ValueFromPipelineByPropertyName = $true)]
    [string] $random_api
)

$ErrorActionPreference = "Stop"
$PSDefaultParameterValues['*:ErrorAction']='Stop'

Write-Host "Login into Docker Registry"
docker login

Write-Host "Switching to score-document folder"
cd src\score-document

Write-Host "Pulling template"
faas-cli template store pull csharp-httprequest

Write-Host "Building score-document function"
faas-cli.exe build -f .\score-document.yml

Write-Host "Tagging $docker_user_name/score-document"
docker tag score-document $docker_user_name/score-document

Write-Host "Pushing $docker_user_name/score-document"
docker push $docker_user_name/score-document

Write-Host "Deploying $docker_user_name/score-document to OpenFaas"
faas-cli.exe deploy --image=$docker_user_name/score-document --name=score-document -g $openfaas_gateway --env="RANDOM_ORG_API_KEY=$random_api"

Write-Host "Switching to UI folder"
cd ..\mureui

Write-Host "Building mureui function"
faas-cli.exe build -f .\mureui.yml

Write-Host "Tagging $docker_user_name/mureui"
docker tag mureui $docker_user_name/mureui

Write-Host "Pushing $docker_user_name/mureui"
docker push $docker_user_name/mureui

Write-Host "Deploying $docker_user_name/mureui to OpenFaas"
faas-cli.exe deploy --image=$docker_user_name/mureui --name=mureui -g $openfaas_gateway


cd ..\..
