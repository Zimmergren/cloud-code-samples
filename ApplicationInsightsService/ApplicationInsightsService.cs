using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace ApplicationInsightsService
{
    /// <summary>
    /// Application Insights Service.
    /// An example of how to use Application Insights with an ITelemetryProcessor.
    /// </summary>
    public class ApplicationInsightsService
    {
        private readonly TelemetryClient _telemetryClient;
        public ApplicationInsightsService(string instrumentationKey)
        {
            IServiceCollection services = new ServiceCollection();
            services.AddApplicationInsightsTelemetryWorkerService(instrumentationKey);

            // NOTE: We inject our custom ITelemetryProcessor here.
            services.AddApplicationInsightsTelemetryProcessor<AzureStorageDependencyProcessor>();

            IServiceProvider serviceProvider = services.BuildServiceProvider();
            _telemetryClient = serviceProvider.GetRequiredService<TelemetryClient>();
        }

        public void TrackException(Exception exceptionTelemetry, Dictionary<string, string> properties = null, Dictionary<string, double> metrics = null)
        {
            _telemetryClient.TrackException(exceptionTelemetry, properties, metrics);
            _telemetryClient.Flush();
        }

        public void TrackTrace(string message, Dictionary<string, string> properties = null)
        {
            if (properties == null)
                _telemetryClient.TrackTrace(message);
            else
                _telemetryClient.TrackTrace(message, properties);

            _telemetryClient.Flush();
        }


        public void TrackRequest(ApplicationInsightsRequestTelemetry requestTelemetry)
        {
            if (requestTelemetry == null)
                throw new ArgumentNullException(nameof(requestTelemetry));

            RequestTelemetry request = new RequestTelemetry(requestTelemetry.Name, requestTelemetry.StartTime, requestTelemetry.Duration, requestTelemetry.ResponseCode, requestTelemetry.Success);
            foreach (var key in requestTelemetry.Properties.Keys)
            {
                request.Properties.Add(key, requestTelemetry.Properties[key]);
            }
            request.Url = requestTelemetry.Url;
            _telemetryClient.TrackRequest(request);
            _telemetryClient.Flush();
        }

        public void TrackMetrics(ApplicationInsightsMetricTelemetry metricTelemetry)
        {
            if (metricTelemetry == null)
                throw new ArgumentNullException(nameof(metricTelemetry));

            var metric = new MetricTelemetry(metricTelemetry.MetricName, metricTelemetry.MetricValue);
            foreach (var prop in metricTelemetry.Properties)
            {
                metric.Properties.Add(prop.Key, prop.Value);
            }
            _telemetryClient.TrackMetric(metric);
            _telemetryClient.Flush();
        }
    }
}
