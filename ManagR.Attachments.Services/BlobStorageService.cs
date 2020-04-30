using ManagR.Attachments.Services.Interfaces;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Threading.Tasks;

namespace ManagR.Attachments.Services
{
    public class BlobStorageService : IBlobStorageService
    {
        public async Task<string> GetContainerSasUri(string storedPolicyName = null)
        {
            const string ConnectionString = "DefaultEndpointsProtocol=https;AccountName=managrattachements;AccountKey=KqCWT/Go8Z3/3DqBDPEAmINOiUOi71hK6P8ayDGV4a1AY/Tn89CgaNoGdMREqYuRgIn4ZFi/HGzQk406h5TIrA==";
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConnectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();

            // get a reference to the container for the shared access signature
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
            return "?sv=2019-02-02&ss=b&srt=sco&sp=rwdlac&se=2020-06-24T19:55:21Z&st=2020-04-14T11:55:21Z&spr=https&sig=NvcZ0ctPtvzxqsPJwKi97PL1dSRTlCqPxTI6bKZWkOc%3D";
            // return sasContainerToken;
        }
    }
}
