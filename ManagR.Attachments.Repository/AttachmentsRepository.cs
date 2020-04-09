using ManagR.Attachments.Models.ViewModels;
using ManagR.Attachments.Models.Dtos;
using ManagR.Attachments.Repository.Interfaces;
using System;
using System.Threading.Tasks;
using ManagR.Attachments.Data;
using System.Collections.Generic;
using ManagR.Attachments.Services.Interfaces;

namespace ManagR.Attachments.Repository
{
    public class AttachmentsRepository : IAttachmentsRepository
    {
        private AttachmentsDb _context;
        private IBlobStorageService _blobStorageService;
        public AttachmentsRepository(AttachmentsDb context, IBlobStorageService blobStorageService)
        {
            _context = context;
        }
        public async Task<PreparedFileVm> PrepareFileUpload(UploadFilesVm files)
        {
            var preparedFiles = new List<FileVm>();
            foreach(var file in files.Files)
            {
                // create file dto
                var fileId = Guid.NewGuid();
                var fileMetadata = new FileDto
                {
                    Id = fileId,
                    LastModifiedDate = file.LastModifiedDate,
                    Name = file.Name,
                    Size = file.Size,
                    Status = FileStatus.Uploading,
                    Type = file.Type,
                    UploadedBy = files.UploadedBy,
                    UploadedOn = DateTime.Now,
                    UploaderId = files.UploaderId
                };

                //file.Id = fileId;

                //preparedFiles.Add(file);

                _context.Attachments.Add(fileMetadata);
            }

            await _context.SaveChangesAsync();
            var sas = _blobStorageService.GetSasToken();
            return new PreparedFileVm
            {
                Files = preparedFiles,
                SasToken = sas
            };
        }
    }
}
