module kv 'keyvault.bicep' = {
  name: 'myKeyVault'
}

module storage 'storage.bicep' = {
  name: 'myStorage'
}