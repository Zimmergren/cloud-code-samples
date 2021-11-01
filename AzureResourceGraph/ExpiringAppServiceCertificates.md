# Get expiring App Service Certificates
These queries are some example queries I portrayed in a blog entry to quickly discover any upcoming certificates that expire in Azure App Services.
- Read more: https://zimmergren.net/discover-expiring-azure-app-services-certificates


## Example Queries
 
- Get all certificates:
```kusto
resources
| where type == "microsoft.web/certificates"
```

- Limit the returned properties:

```kusto
resources
| where type == "microsoft.web/certificates"
| project resourceGroup, name, subscriptionId, properties.expirationDate, properties.thumbprint, properties.subjectName, properties.issuer
```

- Order by expiration date:

```kusto
resources
| where type == "microsoft.web/certificates"
// Extend the expiration date, enabling us to easier sort and filter by it.
| extend ExpirationDate = todatetime(properties.expirationDate)
| project ExpirationDate, resourceGroup, name, subscriptionId, properties.expirationDate, properties.thumbprint, properties.subjectName, properties.issuer
| order by ExpirationDate asc
```

- Get certificates that expire in 60 days:

```kusto
resources
| where type == "microsoft.web/certificates"
// Extend the expiration date, enabling us to easier sort and filter by it.
| extend ExpirationDate = todatetime(properties.expirationDate)
| project ExpirationDate, resourceGroup, name, subscriptionId, properties.expirationDate, properties.thumbprint, properties.subjectName, properties.issuer
| where ExpirationDate < now() + 60d
| order by ExpirationDate asc
```

- Get number of days until expiration, for readability:

```kusto
resources
| where type == "microsoft.web/certificates"
| extend ExpirationDate = todatetime(properties.expirationDate)
| extend DaysUntilExpiration = datetime_diff("day", ExpirationDate, now())
| project DaysUntilExpiration, ExpirationDate, resourceGroup, name, subscriptionId, properties.expirationDate, properties.thumbprint, properties.subjectName, properties.issuer
| where ExpirationDate < now() + 60d
| order by DaysUntilExpiration
```