resource kv 'Microsoft.KeyVault/vaults@2020-04-01-preview' = {
  location: 'westeurope'
  name: 'myKeyVaultDemoFromBicep1'
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

output keyVaultUri string = kv.properties.vaultUri
output keyVaultSkuName string = kv.properties.sku.name