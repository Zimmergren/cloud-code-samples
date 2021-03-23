// Scope
targetScope = 'resourceGroup'

// Parameters
@minLength(3)
@maxLength(18)
param resourceName string

// Variables
var locationShortName = 'weu'
var locationLongName = 'westeurope'
var resourcesMetadata = {
  name: '{0}-${locationShortName}-${replace(resourceName, '-', '')}'
  location: locationLongName
}

// Resource: Key Vault
module kv './keyvault.bicep' = {
  name: 'keyVaultModule'
  params:{
    location: resourcesMetadata.location
    name: replace(resourcesMetadata.name, '{0}', 'kv')
  }
}

// Resource: Storage Account
module storage './storage.bicep' = {
  name: 'storageModule'
  params: {
    location: resourcesMetadata.location
    name: replace(resourcesMetadata.name, '{0}', 'st')
  }
}

// output objects.
output deploymentResourceGroup object = resourceGroup()