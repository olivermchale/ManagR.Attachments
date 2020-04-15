using System;
using System.Collections.Generic;
using System.Text;

namespace ManagR.Attachments.Models.ViewModels
{
    public class PreparedFileVm
    {
        public string SasToken { get; set; }
        public List<FileVm> Files { get; set; }
    }
}
