// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

// Dummy samples of secrets in code. CredScan should hopefully pick this up.
var test1 = "DefaultEndpointsProtocol=http;AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;TableEndpoint=http://127.0.0.1:10002/devstoreaccount1;";
var test2 = "jdbc:postgresql://mydb.b5uacpxznijm.us-west-2.rds.amazonaws.com:5432/ebdb?user=username&password=mypassword";
var sasTest = "BlobEndpoint=https://sastestdummy.blob.core.windows.net/;QueueEndpoint=https://sastestdummy.queue.core.windows.net/;FileEndpoint=https://sastestdummy.file.core.windows.net/;TableEndpoint=https://sastestdummy.table.core.windows.net/;SharedAccessSignature=sv=2021-06-08&ss=bfqt&srt=s&sp=rwdlacupiytfx&se=2022-11-01T02:08:27Z&st=2022-10-31T18:08:27Z&spr=https&sig=aEpff6lffaCfC2fiLvfOf%2FfP6f7rKyftJGfAdnfgf4wg%3D";

var azureTableStorageDummy = "DefaultEndpointsProtocol=https;AccountName=foobardumdum;AccountKey=h1QWNse3ydtVL2EGTreEMtldl7R132T1poLQdzXpV/t0j84lQphjf2MP78HrtiSYyQu6PTvAda0s+AStr+W4wg==;EndpointSuffix=core.windows.net";