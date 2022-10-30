targetScope = 'resourceGroup'

//
// PARAMETERS
//

// A parameter
param projectFriendlyName string

// A parameter with constraints
@minLength(3)
@maxLength(18)
param resourceName string

// A secure parameter
param apiKey string {
  secure: true
}

// 
// VARIABLES
// 

var locationShortName = 'weu'
var locationLongName = 'westeurope'

var resourcesMetadata = {
  name: '{0}-${locationShortName}-${replace(resourceName, '-', '')}'
  location: locationLongName
}

//
// RESOURCES
// 

// Resource: Define an Azure Storage Account
resource st1 'Microsoft.Storage/storageAccounts@2019-06-01' = {
  // replace hyphens (not allowed in storage account names), replace type with "st" to follow our naming practices.
  name: replace(replace(resourcesMetadata.name, '{0}', 'st'), '-', '')
  location: resourcesMetadata.location
  kind: 'StorageV2'
  sku: {
    name: 'Standard_LRS'
  }
}

// Resource: Define an Azure Key Vault
resource kv 'Microsoft.KeyVault/vaults@2019-09-01' = {
  location: resourcesMetadata.location
  name: replace(resourcesMetadata.name, '{0}', 'kv')
  tags: {
    'demo-delete': 'true'
  }
  properties: {
    tenantId: subscription().tenantId
    sku: {
      family: 'A'
      name: 'standard'
    }
    accessPolicies: [
    ]
  }
}

// Output variables from resources.
output keyVaultUri string = kv.properties.vaultUri
output keyVaultSkuName string = kv.properties.sku.name

output storageAccountName string = st1.name
output storageAccountSku string = st1.sku.name

// output objects.
output deploymentResourceGroup object = resourceGroup()