resource storage1 'Microsoft.Storage/storageAccounts@2020-08-01-preview' = {
  name: 'tobiasStorageDemoBicep123'
  location: 'westeurope'
  sku: {
    name: 'Standard_LRS'
  }
  kind: 'StorageV2'
  properties: {
    supportsHttpsTrafficOnly: true
    accessTier: 'Hot'
    allowBlobPublicAccess: false
    allowSharedKeyAccess: false
    minimumTlsVersion: 'TLS1_2'
    networkAcls: {
      defaultAction: 'Deny'
      ipRules:[
        {
          action: 'Allow'
          value: '152.44.26.251'
        }
      ]
    }
  }
}