using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace CallRequestResponseService
{
    class Program
    {
        private static string apiKey, deploymentModelName;
        static void Main(string[] args)
        {
            // NOTE: For the demo code here, I'm using a secrets.json file that looks like this: 
            /*
             {
              "aistudio-deployment-apikey": "your api key",
              "aistudio-deployment-name": "your model name"
             }

            */


            // Build the configuration
            IConfiguration configuration = new ConfigurationBuilder()
                .AddUserSecrets<Program>()
                .Build();

            deploymentModelName = configuration["aistudio-deployment-name"];
            apiKey = configuration["aistudio-deployment-apikey"];

            InvokeRequestResponseService().Wait();
        }

        static async Task InvokeRequestResponseService()
        {
            var handler = new HttpClientHandler()
            {
                ClientCertificateOptions = ClientCertificateOption.Manual,
                ServerCertificateCustomValidationCallback =
                        (httpRequestMessage, cert, cetChain, policyErrors) => { return true; }
            };
            using (var client = new HttpClient(handler))
            {
                Console.WriteLine("Enter any text to summarize. When you're done, hit Enter:");
                Console.WriteLine("-------------------");

                // For demo purposes.
                // You should protect the strings and ensure proper encoding to accept more complext text, including quotation marks, etc.
                string userInput = Console.ReadLine();

                string requestBody =
                    $@"{{
                        ""inputs"": ""{userInput}""
                    }}";

                // Replace this with the primary/secondary key or AMLToken for the endpoint
                if (string.IsNullOrEmpty(apiKey))
                {
                    throw new Exception("A key should be provided to invoke the endpoint");
                }
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
                client.BaseAddress = new Uri("https://tobias-3655-toaji.westeurope.inference.ml.azure.com/score");

                var content = new StringContent(requestBody);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                // This header will force the request to go to a specific deployment.
                // Remove this line to have the request observe the endpoint traffic rules
                content.Headers.Add("azureml-model-deployment", deploymentModelName);

                HttpResponseMessage response = await client.PostAsync("", content);
                
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Result: {0}", result);
                }
                else
                {
                    Console.WriteLine(string.Format("The request failed with status code: {0}", response.StatusCode));

                    // Print the headers - they include the requert ID and the timestamp,
                    // which are useful for debugging the failure
                    Console.WriteLine(response.Headers.ToString());

                    string responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(responseContent);
                }
            }
        }
    }
}