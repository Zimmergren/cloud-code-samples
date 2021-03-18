using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

namespace ApplicationInsightsService
{
    public class AzureStorageDependencyProcessor : ITelemetryProcessor
    {
        private ITelemetryProcessor Next { get; set; }

        public AzureStorageDependencyProcessor(ITelemetryProcessor next)
        {
            Next = next;
        }

        public void Process(ITelemetry item)
        {
            if (!VerifyDepdendencyIsValid(item)) 
                return; 

            Next.Process(item);
        }

        private bool VerifyDepdendencyIsValid(ITelemetry item)
        {
            DependencyTelemetry dependency = item as DependencyTelemetry;

            if (dependency == null) 
                return true;

            // Here you can filter on your dependency any way you want. 
            // This is an example of how to not report "Azure table" related dependency issues as failures.
            // NOTE: Depending on your production scenarios, you should modify the logic here, and test it.
            // This is meant to illustrate how to accomplish the task, not solve production issues.
            if (dependency.Type == "Azure table" && dependency.Success == false)
            {
                dependency.Success = true;
            }

            return dependency.Success != true;
        }
    }
}
