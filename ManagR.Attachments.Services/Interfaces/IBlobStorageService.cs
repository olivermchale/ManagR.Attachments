using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ManagR.Attachments.Services.Interfaces
{
    public interface IBlobStorageService
    {
        public Task<string> GetContainerSasUri(string storedPolicyName = null);
    }
}
