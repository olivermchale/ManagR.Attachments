using ManagR.Attachments.Models.ViewModels;
using ManagR.Attachments.Models.Dtos;
using ManagR.Attachments.Repository.Interfaces;
using System;
using System.Threading.Tasks;
using ManagR.Attachments.Data;
using System.Collections.Generic;
using ManagR.Attachments.Services.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ManagR.Attachments.Repository
{
    public class AttachmentsRepository : IAttachmentsRepository
    {
        private AttachmentsDb _context;
        private IBlobStorageService _blobStorageService;
        public AttachmentsRepository(AttachmentsDb context, IBlobStorageService blobStorageService)
        {
            _context = context;
            _blobStorageService = blobStorageService;
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
                    ItemId = files.ItemId,
                    Name = file.Name,
                    Size = file.Size,
                    Status = FileStatus.Uploading,
                    Type = file.Type,
                    UploadedBy = files.UploadedBy,
                    UploadedOn = DateTime.Now,
                    UploaderId = files.UploaderId
                };

                preparedFiles.Add(new FileVm
                {
                    Id = fileId,
                    Name = file.Name,
                    Size = file.Size,
                    Type = file.Type
                });

                await _context.Attachments.AddAsync(fileMetadata);              
            }
            await _context.SaveChangesAsync();
            var sas = await _blobStorageService.GetContainerSasUri();
            return new PreparedFileVm
            {
                Files = preparedFiles,
                SasToken = sas
            };
        }

        public async Task<List<AttachmentVm>> GetAttachments(Guid id)
        {
            try
            {
                return await _context.Attachments.Where(a => a.ItemId == id)
                    .Select(i => new AttachmentVm
                    {
                        Id = i.Id,
                        Name = i.Name,
                        Size = i.Size,
                        Status = i.Status,
                        UploadedBy = i.UploadedBy,
                        UploadedOn = i.UploadedOn,
                        UploaderId = i.UploaderId
                    }).ToListAsync();
            }
            catch (Exception e)
            {
                // exception getting attachments....
            }
            return null;
        }

        public async Task<bool> UpdateAttachmentStatus(UpdateStatusVm status)
        {
            try
            {
                var attachment = await _context.Attachments.Where(a => a.Id == status.Id).FirstOrDefaultAsync();
                if (attachment != null)
                {
                    attachment.Status = status.Status;
                    await _context.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception e)
            {
                // exception updating attachment status
            }
            return false;
        }
    }
}
