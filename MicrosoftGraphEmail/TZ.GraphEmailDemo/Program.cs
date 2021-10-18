// #####################################################
// DEMO: Sending e-mails programmatically using Microsoft Graph.
// by Tobias Zimmergren | zimmergren.net | @zimmergren
// Reference blog post: https://zimmergren.net/sending-e-mails-using-microsoft-graph-using-dotnet
// 
// This project template is based on .NET 6.
// See https://aka.ms/new-console-template for more information
// #####################################################

// DEPENDENCIES
using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph;


// DEV EXPERIENCE: Loading the secrets from the secrets.json file. 
// To run this project, ensure you have this file. Please see the reference blog post for instructions: https://zimmergren.net/sending-e-mails-using-microsoft-graph-using-dotnet
var config = new ConfigurationBuilder()
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddUserSecrets<Program>()
    .Build();

//
// NUGET: Azure.Identity
//

// IF YOU ARE USING A CLIENT ID / SECRET:
// Define your credentials based on the created app and user details.
// Specify the options. In most cases we're running the Azure Public Cloud.
var credentials = new ClientSecretCredential(
    config["GraphMail:TenantId"],
    config["GraphMail:ClientId"],
    config["GraphMail:ClientSecret"],
    new TokenCredentialOptions { AuthorityHost = AzureAuthorityHosts.AzurePublicCloud });

// IF YOU ARE USING MANAGED IDENTITY.
//var credentials = new DefaultAzureCredential();

// 
// NUGET: Microsoft.Graph
//

// Define our new Microsoft Graph client.
// Use the credentials we specified above.
GraphServiceClient graphServiceClient = new GraphServiceClient(credentials);

// Define something for the message. 
// I'm getting the HTML e-mail template and replacing a few entries, for demonstration purposes. 
// Real-world implementations of this would use a more robust templating experience, with more options.

var subject = $"Sent from demo code at {DateTime.Now.ToString("s")}";
var body =
    System.IO.File.ReadAllText("mailtemplate.html")
    .Replace("{{HEADER_LINK}}", "https://zimmergren.net")
    .Replace("{{HEADER_LINK_TEXT}}", "Hi, I'm Tobias.")
    .Replace("{{HEADLINE}}", "This is a demo!")
    .Replace("{{BODY}}", $"This is the body of the message. Lots of fun things can go in here. Device: {Environment.MachineName}. User: {Environment.UserName}.");

// Define a simple e-mail message.
var message = new Message
{
    Subject = subject,
    Body = new ItemBody
    {
        ContentType = BodyType.Html,
        Content = body
    },
    ToRecipients = new List<Recipient>()
    {
        new Recipient { EmailAddress = new EmailAddress { Address = "blog@zimmergren.net" }}
    }
};

// Send mail as the given user. 
graphServiceClient
    .Users[config["GraphMail:UserObjectId"]]
    .SendMail(message, true)
    .Request()
    .PostAsync().Wait();