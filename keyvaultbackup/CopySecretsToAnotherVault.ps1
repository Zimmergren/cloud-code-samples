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

# 1.3 Loop secrets into PSCustomObjects, making it easy to work with later.
$secretObjects = $originSecretKeys | ForEach-Object {
    Write-Host " - Getting secret value for '$($_)'"
    $secret = az keyvault secret show --name $_ --vault-name $originVault -o json | ConvertFrom-Json
    
    [PSCustomObject]@{
        secretName  = $_;
        secretValue = $secret.value;
    }#endcustomobject.

}#endforeach.

Write-Host "Done fetching secrets..."

# 2. Set the destination subscription id.
Write-Host "Setting destination subscription to: $($destinationSubscriptionId)..."
az account set -s $destinationSubscriptionId

# 2.2 Loop secrets objects, and set secrets in destination vault
Write-Host "Writing all destination secrets to vault: $($destinationVault)"
$secretObjects | ForEach-Object {
    Write-Host " - Setting secret value for '$($_.secretName)'"
    az keyvault secret set --vault-name $destinationVault --name "$($_.secretName)" --value  "$($_.secretValue)" --disabled $disableDestinationSecrets -o none
}

# 3. Clean up
Write-Host "Cleaning up and exiting."
Remove-Variable secretObjects
Remove-Variable originSecretKeys

Write-Host "Finished."