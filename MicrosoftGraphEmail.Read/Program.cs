using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph;

namespace TZ.GraphReadEmailDemo
{
    class Program
    {

        // A simple demonstration of how to read e-mails from an Inbox using the Microsoft Graph
        // The code example uses the Mail.Read permission to access the current user's inbox.

        // Read the blog post: https://zimmergren.net/reading-emails-with-microsoft-graph-using-net/

        static void Main(string[] args)
        {
            /*
             * IMPORTANT:
             * This is demo code only. 
             * Authentication should preferably be done using managed identity, without any credentials in code or config.
             * */

            // Set up the config to load the user secrets
            var config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddUserSecrets<Program>()
                .Build();

            // Define the credentials. 
            // Note: In your implementations of this code, please consider using managed identities, and avoid credentials in code or config.
            var credentials = new ClientSecretCredential(
                config["GraphMail:TenantId"],
                config["GraphMail:ClientId"],
                config["GraphMail:ClientSecret"],
                new TokenCredentialOptions { AuthorityHost = AzureAuthorityHosts.AzurePublicCloud });

            // Define a new Microsoft Graph service client.            
            GraphServiceClient _graphServiceClient = new GraphServiceClient(credentials);

            // Get the e-mails for a specific user.
            var messages = _graphServiceClient.Users["tobias@zimmergren.net"].Messages.Request().GetAsync().Result;
            foreach (var message in messages)
            {
                Console.WriteLine($"{message.ReceivedDateTime?.ToString("yyyy-MM-dd HH:mm:ss")} from {message.From.EmailAddress.Address}");
                Console.WriteLine($"{message.Subject}");
                Console.WriteLine("---");
            }
        }
    }
}