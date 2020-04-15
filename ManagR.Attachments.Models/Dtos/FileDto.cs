using ManagR.Attachments.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ManagR.Attachments.Models.Dtos
{
    public class FileDto
    {
        public Guid Id { get; set; }
        public Guid ItemId { get; set; }
        public DateTime UploadedOn { get; set; }
        public Guid UploaderId { get; set; }
        public string UploadedBy { get; set; }
        public string Name { get; set; }
        public int Size { get; set; }
        public string Type { get; set; }
        public FileStatus Status { get; set; }
    }
}
