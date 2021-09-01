using LogAnalyticsDeepLinkGenerator;
using System;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {

            string lawQuery = @"AppExceptions
                                | where ProblemId != 'System.InvalidOperationException'
                                | summarize count() by ProblemId
                                | order by count_ desc
                                | render columnchart ";

            // Simple call to demonstrate.
            var lawUrl = LinkGenerator.GetLogAnalyticsDeepLink(
                lawQuery,
                "SUBSCRIPTION_ID",
                "RESOURCE_GROU_NAME",
                "LOG_ANALYTICS_WORKSPACE_NAME",
                QueryTimeframe.P1W);

            Console.WriteLine("LOG ANALYTICS LINK:");
            Console.WriteLine(lawUrl);

            var appiQuery = @"exceptions
                            | where problemId != 'System.InvalidOperationException'
                            | summarize count() by problemId
                            | order by count_ desc
                            | render columnchart";

            // Simple call to demonstrate.
            var appiUrl = LinkGenerator.GetApplicationInsightsDeepLink(
                appiQuery,
                "TENANT_ID",
                "SUBSCRIPTION_ID",
                "RESOURCE_GROUP_NAME",
                "APPLICATION_INSIGHTS_RESOURCE_NAME",
                QueryTimeframe.P1W);

            Console.WriteLine();
            Console.WriteLine("APP INSIGHTS LINK:");
            Console.WriteLine(appiUrl);
        }
    }
}