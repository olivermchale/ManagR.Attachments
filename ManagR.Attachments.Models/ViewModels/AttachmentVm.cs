using System;
using System.Collections.Generic;
using System.Text;

namespace ManagR.Attachments.Models.ViewModels
{
    public class AttachmentVm
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Size { get; set; }
        public FileStatus Status { get; set; }
        public Guid UploaderId { get; set; }
        public string UploadedBy { get; set; }
        public DateTime UploadedOn { get; set; }
    }
}
