using ManagR.Attachments.Services.Interfaces;
using Microsoft.WindowsAzure.Storage;
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
    }
}
