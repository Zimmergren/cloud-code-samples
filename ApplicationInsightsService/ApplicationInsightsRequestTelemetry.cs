using System;
using System.Collections.Generic;

namespace ApplicationInsightsService
{
    public class ApplicationInsightsRequestTelemetry
    {
        public ApplicationInsightsRequestTelemetry(string name, DateTime startTime, TimeSpan duration, string responseCode, bool success)
        {
            this.Properties = new Dictionary<string, string>();

            this.Name = name;
            this.StartTime = startTime;
            this.Duration = duration;
            this.ResponseCode = responseCode;
            this.Success = success;
        }

        public string Name { get; private set; }

        public DateTime StartTime { get; private set; }

        public TimeSpan Duration { get; private set; }

        public string ResponseCode { get; private set; }

        public bool Success { get; private set; }

        public Dictionary<string, string> Properties { get; set; }

        public Uri Url { get; set; }
    }
}