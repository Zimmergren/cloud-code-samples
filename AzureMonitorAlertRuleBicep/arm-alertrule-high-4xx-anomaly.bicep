param alertRuleName string = 'Web App has 4xx anomalies'
param webAppResourceId string = 'THE WEB APP RESOURCE ID'
param actionGroupResourceId string = 'AN EXISTING ACTION GROUP RESOURCE ID'

resource metricalerts_PROD_WEU_4xx_Anomalies 'microsoft.insights/metricalerts@2018-03-01' = {
  name: alertRuleName
  location: 'global'
  properties: {
    severity: 1
    enabled: true
    scopes: [
      webAppResourceId
    ]
    evaluationFrequency: 'PT5M'
    windowSize: 'PT5M'
    criteria: {
      allOf: [
        {
          threshold: 1
          name: 'Metric1'
          metricNamespace: 'Microsoft.Web/sites'
          metricName: 'Http4xx'
          operator: 'GreaterThan'
          timeAggregation: 'Total'
          criterionType: 'StaticThresholdCriterion'
        }
      ]
      'odata.type': 'Microsoft.Azure.Monitor.MultipleResourceMultipleMetricCriteria'
    }
    autoMitigate: true
    targetResourceType: 'Microsoft.Web/sites'
    actions: [
      {
        actionGroupId: actionGroupResourceId
        webHookProperties: {}
      }
    ]
  }
}
