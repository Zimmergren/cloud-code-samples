param location string
param name string

// Resource: Define an Azure Key Vault
resource kv 'Microsoft.KeyVault/vaults@2019-09-01' = {
  location: location
  name: name
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
