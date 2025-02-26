using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using System.Text.RegularExpressions;

namespace ApplicationInsights.RedactSensitiveInformation
{
    /// <summary>
    /// Redacts standardized sensitive information from the trace messages.
    /// </summary>
    internal class SensitivityRedactionTelemetryInitializer : ITelemetryInitializer
    {
        public void Initialize(ITelemetry t)
        {
            var traceTelemetry = t as TraceTelemetry;
            if (traceTelemetry != null)
            {
                // Use Regex to replace any e-mail address with a replacement string.
                traceTelemetry.Message = Regex.Replace(traceTelemetry.Message, @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", "[PII REDACTED]");
                
                // If we don't remove this CustomDimension, the telemetry message will still contain the PII in the "OriginalFormat" property.
                traceTelemetry.Properties.Remove("OriginalFormat");
            }
        }
    }
}
