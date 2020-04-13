using ManagR.Attachments.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ManagR.Attachments.Repository.Interfaces
{
    public interface IAttachmentsRepository
    {
        public Task<PreparedFileVm> PrepareFileUpload(UploadFilesVm files);

        public Task<List<AttachmentVm>> GetAttachments(Guid id);
    }
}
