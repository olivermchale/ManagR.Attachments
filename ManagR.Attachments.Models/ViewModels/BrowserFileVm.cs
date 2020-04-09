using System;
using System.Collections.Generic;
using System.Text;

namespace ManagR.Attachments.Models.ViewModels
{
    public class BrowserFileVm
    {
        public string Name { get; set; }
        public int LastModified { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public int Size { get; set; }
        public string Type { get; set; }
    }
}
