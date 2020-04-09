using System;
using System.Collections.Generic;
using System.Text;

namespace ManagR.Attachments.Models.ViewModels
{
    public class UploadFilesVm
    {
        public Guid UploaderId { get; set; }
        public string UploadedBy { get; set; }
        public List<BrowserFileVm> Files { get; set; }
    }
}
