using Azure.Storage.Blobs;
using System.Text;


/*
 To set local variable
Windows:  setx AZURE_STORAGE_CONNECTION_STRING "<yourconnectionstring>"
Linux e mac: export AZURE_STORAGE_CONNECTION_STRING="<yourconnectionstring>"
 */

var conn = Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING");
var containerName = "testimages";

do
{
    Console.WriteLine("What do you want to do?");
    Console.WriteLine("1 - Upload test image");
    Console.WriteLine("2 - List all blobs");

    var choice = Console.ReadLine();

    Console.Clear();

    if (choice == "1")
        await UploadImage();
    else if (choice == "2")
        await ListBlobs();


} while (true);


async Task ListBlobs()
{
    var blobServiceClient = new BlobServiceClient(conn);
    var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
    var output = new StringBuilder();
    var qtdItems = 0;

    await foreach (var blotItem in containerClient.GetBlobsAsync())
    {
        qtdItems++;
        output.AppendLine($"{qtdItems}: {blotItem.Name}");
        output.AppendLine($"        Size: {blotItem.Properties.ContentLength}");
        output.AppendLine("");
    }

    Console.WriteLine($"Listing {qtdItems} blobs");
    Console.WriteLine(output.ToString());
}


async Task UploadImage()
{
    var fileLocalPath = Path.Combine(Directory.GetCurrentDirectory(), "Files/mt07_base64.txt");
    var fileContentBytes = Convert.FromBase64String(File.ReadAllText(fileLocalPath));

    var fileName = Guid.NewGuid().ToString() + ".jpg";

    var blobClient = new BlobClient(conn, containerName, fileName);

    Console.WriteLine("-- upload started --");

    using (var stream = new MemoryStream(fileContentBytes))
    {
        await blobClient.UploadAsync(stream);
    }

    Console.WriteLine("-- image uploaded --");
    Console.WriteLine("-- file name:" + fileName);
    Console.WriteLine("-- URL: " + blobClient.Uri.AbsoluteUri);
}