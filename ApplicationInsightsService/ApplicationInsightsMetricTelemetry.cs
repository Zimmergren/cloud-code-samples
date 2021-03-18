using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationInsightsService
{
    public class ApplicationInsightsMetricTelemetry
    {
        public ApplicationInsightsMetricTelemetry(string metricName, double metricValue)
        {
            this.Properties = new Dictionary<string, string>();
            this.MetricName = metricName;
            this.MetricValue = metricValue;
        }

        public string MetricName { get; }
        public double MetricValue { get; }
        public IDictionary<string, string> Properties { get; set; }
    }
}
