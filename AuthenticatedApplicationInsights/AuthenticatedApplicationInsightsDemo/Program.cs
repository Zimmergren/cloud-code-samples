using Azure.Identity;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using System;

namespace AuthenticatedApplicationInsightsDemo
{
    class Program
    {
        // Read the blog: https://zimmergren.net/enforce-authentication-when-sending-application-insights-telemetry/ 

        // For demo purposes.
        // I am deliberetly disclosing my telemetry key here since my Application Insights only accept authenticated and authorized requests.
        private const string appiKey = "d85d8694-484c-4f8e-a261-ac9dd232146e";
        private const string appiEndpoint = "https://westeurope-5.in.applicationinsights.azure.com/";

        static void Main(string[] args)
        {
            //
            // Preparations.
            //

            // Construct our connection string - or just copy it from the Application Insights instance entirely.
            var appiConnectionString = $"InstrumentationKey={appiKey};IngestionEndpoint={appiEndpoint}";

            //
            // Wire up a new Application Insights client
            //
            // - Define the TelemetryConfiguration with a connections tring
            // - Define the identity to use. For development purposes, I'm using DefaultAzureCredentials(). In production/cloud workloads, this would be connected using ManagedIdentityCredentials in most of my scenarios.
            // -- Make sure your currently signed in user to VS has access to Application Insights, else it will not work, as it requires an authenticated user.
            // - Set up a new TelemetryClient, use our configuration, and send some traces.
            

            // required nuget: Microsoft.ApplicationInsights
            var appiConfig = new TelemetryConfiguration
            {
                ConnectionString = appiConnectionString
            };

            // required nuget: Azure.Identity
            var credential = new DefaultAzureCredential(); // For dev: Your VS user - make sure it has RBAC configured with the "Monitoring Metrics Publisher" role for your Application Insights instance.
            appiConfig.SetAzureTokenCredential(credential);

            var telemetryClient = new TelemetryClient(appiConfig);

            // Send some telemetry, to ensure they end up in the logs as we expect.
            telemetryClient.TrackTrace($"I code. Therefore I am.");
            telemetryClient.TrackException(new System.Exception("Unicorns does not exist"));
            telemetryClient.Flush();

            Console.WriteLine("Done sending some telemetry...");
        }
    }
}
