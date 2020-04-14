using System;
using System.Collections.Generic;
using System.Text;

namespace ManagR.Attachments.Models.ViewModels
{
    public class UpdateStatusVm
    {
        public Guid Id { get; set; }
        public FileStatus Status { get; set; }
    }
}
