// Necessary using statements.
using ApplicationInsights.RedactSensitiveInformation;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.WorkerService;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.ApplicationInsights;

// DEMO ONLY: Don't put credentials in code - use Azure Key Vault, or applicable protected configuration services.
// I am using the connection string in code for clarity and avoiding unnecessary logic that distracts from the focus of the demo.
const string connectionString = "InstrumentationKey=<GUID>;IngestionEndpoint=https://<endpoint>.in.applicationinsights.azure.com/;LiveEndpoint=https://<endpoint>.livediagnostics.monitor.azure.com/";

#region Wire-up

//
// Wire-up.
// 
IServiceCollection services = new ServiceCollection();

// Add ApplicationInsightsLoggerProvider logger.
services.AddLogging(loggingBuilder => loggingBuilder.AddFilter<ApplicationInsightsLoggerProvider>("Category", LogLevel.Information));

// Add Application Insights logic (ApplicationInsightsTelemetryWorkerService)
services.AddApplicationInsightsTelemetryWorkerService((ApplicationInsightsServiceOptions options) => options.ConnectionString = connectionString);


// NOTE: Injecting the SensitivityRedaction initializer.
services.AddSingleton<ITelemetryInitializer, SensitivityRedactionTelemetryInitializer>();


IServiceProvider serviceProvider = services.BuildServiceProvider();

#endregion


//
// NOTE: Program logic to demonstrate
//

// Get the app insights ILogger from the service provider. 
ILogger<Program> logger = serviceProvider.GetRequiredService<ILogger<Program>>();


// Sending a few log messages. Some include PII, some does not. 
logger.LogWarning("This is a log message without PII.");
logger.LogWarning("This is a log message with an e-mail: spam@zimmergren.net");
logger.LogWarning("This is another message with spam@zimmergren.net, and random@unicorn-demo-haiku.ai");
logger.LogWarning("Users access restrictions changed for: name@thecompany123.io;another@mail.com, new access level is 'Reader' on resource '123'");


// For demo purposes in our console app. 
// Used to directly flush the buffer before we quit the app.
var telemetryClient = serviceProvider.GetRequiredService<TelemetryClient>();
telemetryClient.Flush();
Task.Delay(5000).Wait();
           
