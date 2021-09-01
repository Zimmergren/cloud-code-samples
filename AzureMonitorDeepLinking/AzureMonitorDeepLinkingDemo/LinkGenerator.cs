using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Web;

namespace LogAnalyticsDeepLinkGenerator
{
    public class LinkGenerator
    {
        /// <summary>
        /// Gets a direct/deep-link URL to a specific KQL query in an Azure Log Analytics workspace.
        /// </summary>
        /// <param name="kqlQuery">The plain-text query.</param>
        /// <param name="subscriptionId">The Azure Subscription ID.</param>
        /// <param name="resourceGroup">The Resource Group Nme.</param>
        /// <param name="workspaceId">The Log Analytics Workspace ID.</param>
        /// <param name="timeFrame">The Timeframe for the query.</param>
        /// <returns></returns>
        public static string GetLogAnalyticsDeepLink(string kqlQuery, string subscriptionId, string resourceGroup, string workspaceId, QueryTimeframe timeFrame)
        {
            // Encode the query with our params.
            var encodedQuery = GetEncodedQuery(kqlQuery);

            // Return the URL with the query embedded, and put the timespan at the end.
            return $"https://portal.azure.com/#blade/Microsoft_OperationsManagementSuite_Workspace/AnalyticsBlade/initiator/AnalyticsShareLinkToQuery/isQueryEditorVisible/true/scope/%7B%22resources%22%3A%5B%7B%22resourceId%22%3A%22%2Fsubscriptions%2F{subscriptionId}%2Fresourcegroups%2F{resourceGroup}%2Fproviders%2Fmicrosoft.operationalinsights%2Fworkspaces%2F{workspaceId}%22%7D%5D%7D/query/{encodedQuery}/isQueryBase64Compressed/true/timespanInIsoFormat/{timeFrame}";
        }

        /// <summary>
        /// Gets a direct/deep-link URL to a specific KQL query in an Azure Application Insights resource.
        /// </summary>
        /// <param name="kqlQuery">The plain-text query.</param>
        /// <param name="tenantId">The Azure directory/tenant ID.</param>
        /// <param name="subscriptionId">The Azure Subscription ID.</param>
        /// <param name="resourceGroup">The Resource Group Nme.</param>
        /// <param name="appInsightsName">The Azure Application Insights resource name.</param>
        /// <param name="timeFrame">The Timeframe for the query.</param>
        /// <returns></returns>
        public static string GetApplicationInsightsDeepLink(string kqlQuery, string tenantId, string subscriptionId, string resourceGroup, string appInsightsName, QueryTimeframe timeFrame)
        {
            // Encode the query with our params.
            var encodedQuery = GetEncodedQuery(kqlQuery);

            // Return the URL with the query embedded, and put the timespan at the end.
            return $"https://portal.azure.com#@{tenantId}/blade/Microsoft_Azure_Monitoring_Logs/LogsBlade/resourceId/%2Fsubscriptions%2F{subscriptionId}%2FresourceGroups%2F{resourceGroup}%2Fproviders%2Fmicrosoft.insights%2Fcomponents%2F{appInsightsName}/source/LogsBlade.AnalyticsShareLinkToQuery/q/{encodedQuery}/timespan/{timeFrame}";
        }

        private static string GetEncodedQuery(string kqlQuery)
        {
            string encodedQuery;
            // Encode the query.
            var encodedQueryBytes = Encoding.UTF8.GetBytes(kqlQuery);

            // Compress.
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (GZipStream zipStream = new GZipStream(memoryStream, CompressionMode.Compress))
                {
                    // 1. Compress/Zip the string.
                    zipStream.Write(encodedQueryBytes, 0, encodedQueryBytes.Length);
                    zipStream.Close();
                    var compressedQuery = memoryStream.ToArray();

                    // 2. Base64 encode the string.
                    // 3. UrlEncode the string.
                    encodedQuery = HttpUtility.UrlEncode(Convert.ToBase64String(compressedQuery));
                }
            }

            // 4. Ready. The encoded query that can be passed on to the URL.
            return encodedQuery;
        }
    }

    /// <summary>
    /// Defines the timeframe to include in the query.
    /// Based on the ISO standard. 
    /// In this demo, I am just setting a few pre-defined selections to make the query building easy. Adjust as needed.
    /// </summary>
    public enum QueryTimeframe
    {
        // Minute intervals.
        PT5M,
        PT30M,
        PT60M,

        // One Day.
        P1D,

        // One Week.
        P1W,

        // One Month.
        P1M
    }
}
