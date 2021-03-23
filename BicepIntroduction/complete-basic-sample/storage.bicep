param location string
param name string

// Resource: Define an Azure Storage Account
resource st1 'Microsoft.Storage/storageAccounts@2019-06-01' = {
  // replace hyphens (not allowed in storage account names), replace type with "st" to follow our naming practices.
  name: replace(name, '-', '')
  location: location
  kind: 'StorageV2'
  sku: {
    name: 'Standard_LRS'
  }
}

output storageAccountName string = st1.name
output storageAccountSku string = st1.sku.name