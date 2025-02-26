Param(
    [parameter(mandatory)] [string] $originVault,
    [parameter(mandatory)] [string] $originSubscriptionId,
    [parameter(mandatory)] [string] $destinationVault,
    [parameter(mandatory)] [string] $destinationSubscriptionId,
    [string] $disableDestinationSecrets = $true
)

# 1. Set the source subscription id. 
Write-Host "Setting origin subscription to: $($originSubscriptionId)..."
az account set -s $originSubscriptionId

# 1.1 Get all secrets
Write-Host "Listing all origin secrets from vault: $($originVault)"
$originSecretKeys = az keyvault secret list --vault-name $originVault  -o json --query "[].name"  | ConvertFrom-Json

# 1.3 Loop the secrets, and push the value to the destination vault without instantiating new variables.
$originSecretKeys | ForEach-Object {
    $secretName = $_
    Write-Host " - Getting '$($secretName)' from origin, and setting in destination Vault... '$($destinationVault)'"
    az keyvault secret set --name $secretName --vault-name $destinationVault -o none --value(az keyvault secret show --name $secretName --vault-name $originVault -o json --query value)
}

Write-Host "Finished."
