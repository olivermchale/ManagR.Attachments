using System.Threading.Tasks;
using ManagR.Attachments.Models.ViewModels;
using ManagR.Attachments.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ManagR.Attachments.Controllers
{
    public class AttachmentsController : Controller
    {
        private IAttachmentsRepository _attachmentsRepository;
        public AttachmentsController(IAttachmentsRepository attachmentsRepository)
        {
            _attachmentsRepository = attachmentsRepository;
        }

        [HttpPost]
        public async Task<IActionResult> PrepareAttachments([FromBody] UploadFilesVm files)
        {
            return Ok(_attachmentsRepository.PrepareFileUpload(files));
        }
    }
}