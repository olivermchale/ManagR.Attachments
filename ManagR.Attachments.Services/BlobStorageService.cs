using ManagR.Attachments.Services.Interfaces;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Threading.Tasks;

namespace ManagR.Attachments.Services
{
    public class BlobStorageService : IBlobStorageService
    {
        public string GetSasToken()
        {
            //todo: replace with pipeline variable
            const string ConnectionString = "DefaultEndpointsProtocol=https;AccountName=managrattachements;AccountKey=KqCWT/Go8Z3/3DqBDPEAmINOiUOi71hK6P8ayDGV4a1AY/Tn89CgaNoGdMREqYuRgIn4ZFi/HGzQk406h5TIrA==";
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConnectionString);

            // Create a new access policy for the account.
            SharedAccessAccountPolicy policy = new SharedAccessAccountPolicy()
            {
                Permissions = SharedAccessAccountPermissions.Read | SharedAccessAccountPermissions.Write | SharedAccessAccountPermissions.List,
                Services = SharedAccessAccountServices.Blob | SharedAccessAccountServices.File,
                ResourceTypes = SharedAccessAccountResourceTypes.Service,
                SharedAccessExpiryTime = DateTime.UtcNow.AddHours(24),
                Protocols = SharedAccessProtocol.HttpsOnly
            };

            // Return the SAS token.
            return storageAccount.GetSharedAccessSignature(policy);
        }

        public async Task<string> GetContainerSasUri(string storedPolicyName = null)
        {
            const string ConnectionString = "DefaultEndpointsProtocol=https;AccountName=managrattachements;AccountKey=KqCWT/Go8Z3/3DqBDPEAmINOiUOi71hK6P8ayDGV4a1AY/Tn89CgaNoGdMREqYuRgIn4ZFi/HGzQk406h5TIrA==";
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConnectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();

            // get a reference tot he container for the shared access signature
            var container = blobClient.GetContainerReference("managr");

            SharedAccessBlobPolicy adHocPolicy = new SharedAccessBlobPolicy()
            {
                // When the start time for the SAS is omitted, the start time is assumed to be the time when the storage service receives the request.
                // Omitting the start time for a SAS that is effective immediately helps to avoid clock skew.
                SharedAccessExpiryTime = DateTime.UtcNow.AddHours(24),
                SharedAccessStartTime = DateTime.UtcNow.AddMinutes(-5),
                Permissions = SharedAccessBlobPermissions.Write | SharedAccessBlobPermissions.List | SharedAccessBlobPermissions.Read
            };

            // Generate the shared access signature on the container, setting the constraints directly on the signature.
            var sasContainerToken = container.GetSharedAccessSignature(adHocPolicy, null);
            return "?sv=2019-02-02&ss=b&srt=sco&sp=rwdlac&se=2020-04-13T21:12:21Z&st=2020-04-13T13:12:21Z&spr=https&sig=wQmGLlBtSyT%2Frv8F0%2FT1R93s3Z6bO%2Fa5ZNJObqdpaVk%3D";
            // return sasContainerToken;
        }
    }
}
