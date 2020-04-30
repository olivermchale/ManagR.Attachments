using ManagR.Attachments.Models.Dtos;
using ManagR.Attachments.Models.ViewModels;
using ManagR.Attachments.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ManagR.Attachments.Controllers.Tests
{
    public class AttachmentsControllerTests
    {
        private Mock<IAttachmentsRepository> _mockAttachmentsRepository;
        private AttachmentsController _attachmentsController;
        private FileDto _stubFileDto;
        private UploadFilesVm _stubFiles;
        private UpdateStatusVm _stubStatusUpdate;
        private List<AttachmentVm> _stubAttachmentList;

        [SetUp]
        public void Setup()
        {
            _stubFileDto = new FileDto
            {
                Id = Guid.Parse("aada6f2a-8854-4a5e-a907-2244f43408a1"),
                ItemId = Guid.Parse("5eb09b45-9c70-4465-b62d-535e28b16aed"),
                Name = "Stub file",
                Size = 25424,
                Status = Models.ViewModels.FileStatus.Uploading,
                Type = "Image/PNG",
                UploadedBy = "Stub test",
                UploadedOn = DateTime.Now,
                UploaderId = Guid.Parse("aada6f2a-8854-4a5e-a907-2244f43408a1"),
            };
            _stubFiles = new UploadFilesVm
            {
                Files = new System.Collections.Generic.List<FileMetadataVm>
                {
                    new FileMetadataVm
                    {
                        Name = "Stub file metadata",
                        Size = 1000,
                        Type = "Image/PNG"
                    }
                },
                ItemId = Guid.Parse("aada6f2a-8854-4a5e-a907-2244f43408a2"),
                UploadedBy = "Stub tester",
                UploaderId = Guid.Parse("aada6f2a-8854-4a5e-a907-2244f43408a3"),
            };
            _stubStatusUpdate = new UpdateStatusVm
            {
                Id = Guid.Parse("aada6f2a-8854-4a5e-a907-2244f43408a1"),
                Status = FileStatus.Uploaded,
            };
            _stubAttachmentList = new List<AttachmentVm>
            {
                new AttachmentVm
                {
                    Id = Guid.Parse("aada6f2a-8854-4a5e-a907-2244f43408a3"),
                    Name = "Stub attachment",
                    Size = 100,
                    Status = FileStatus.Failed,
                    UploadedBy = "Stub tester",
                    UploaderId = Guid.Parse("aada6f2a-8854-4a5e-a907-2244f43408a3"),
                    UploadedOn = DateTime.Now
                }
            };

            _mockAttachmentsRepository = new Mock<IAttachmentsRepository>();
            _attachmentsController = new AttachmentsController(_mockAttachmentsRepository.Object);
        }

        [Test]
        public async Task PrepareAttachments_Valid_Success()
        {
            // Arrange
            var files = _stubFiles;

            // Act
            var result = await _attachmentsController.PrepareAttachments(_stubFiles) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 200);
        }

        [Test]
        public async Task GetAttachments_Valid_Success()
        {
            // Arrange
            var Id = Guid.NewGuid();
            _mockAttachmentsRepository.Setup(m => m.GetAttachments(It.IsAny<Guid>()))
                .ReturnsAsync(_stubAttachmentList);

            // Act
            var result = await _attachmentsController.GetAttachments(Id) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 200);
        }

        [Test]
        public async Task UpdateAttachmentStatus_Valid_Success()
        {
            // Arrange
            var status = _stubStatusUpdate;
            _mockAttachmentsRepository.Setup(m => m.UpdateAttachmentStatus(It.IsAny<UpdateStatusVm>()))
                .ReturnsAsync(true);

            // Act
            var result = await _attachmentsController.UpdateAttachmentStatus(status) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 200);
        }

        [Test]
        public async Task UpdateAttachmentStatus_UpdateFails_500()
        {
            // Arrange
            var status = _stubStatusUpdate;
            _mockAttachmentsRepository.Setup(m => m.UpdateAttachmentStatus(It.IsAny<UpdateStatusVm>()))
                .ReturnsAsync(false);

            // Act
            var result = await _attachmentsController.UpdateAttachmentStatus(status) as StatusCodeResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, 500);
        }
    }
}