using ManagR.Attachments.Data;
using ManagR.Attachments.Models.Dtos;
using ManagR.Attachments.Models.ViewModels;
using ManagR.Attachments.Repository.Interfaces;
using ManagR.Attachments.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace ManagR.Attachments.Repository.Tests
{
    public class Tests
    {
        private AttachmentsRepository _attachmentsRepository;
        private Mock<IBlobStorageService> _mockBlobStorageService;
        private Mock<ILogger<AttachmentsRepository>> _mockLogger;
        private FileDto _stubFileDto;
        private UploadFilesVm _stubFiles;
        private UpdateStatusVm _stubStatusUpdate;

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
            _mockBlobStorageService = new Mock<IBlobStorageService>();
            _mockLogger = new Mock<ILogger<AttachmentsRepository>>();
            _attachmentsRepository = new AttachmentsRepository(GetInMemoryContextWithSeedData(), _mockBlobStorageService.Object, _mockLogger.Object);
        }

        private AttachmentsDb GetInMemoryContextWithSeedData()
        {
            var options = new DbContextOptionsBuilder<AttachmentsDb>()
                                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                                .Options;
            var context = new AttachmentsDb(options);
            context.Add(_stubFileDto);
            context.SaveChanges();

            return context;
        }

        [Test]
        public async Task PrepareFileUpload_Valid_Success()
        {
            // Arrange
            _mockBlobStorageService.Setup(m => m.GetContainerSasUri(It.IsAny<string>()))
                .ReturnsAsync("some sas");

            // Act
            var preparedFiles = await _attachmentsRepository.PrepareFileUpload(_stubFiles);

            //  Assert
            Assert.IsNotNull(preparedFiles);
            Assert.AreEqual(preparedFiles.SasToken, "some sas");
            Assert.IsNotNull(preparedFiles.Files[0].Id);
            Assert.AreEqual(preparedFiles.Files[0].Name, _stubFiles.Files[0].Name);
            Assert.AreEqual(preparedFiles.Files[0].Size, _stubFiles.Files[0].Size);
            Assert.AreEqual(preparedFiles.Files[0].Type, _stubFiles.Files[0].Type);
        }

        [Test]
        public async Task GetAttachments_Valid_Success()
        {
            // Arrange
            var itemId = _stubFileDto.ItemId;

            // Act
            var attachments = await _attachmentsRepository.GetAttachments(itemId);

            // Assert
            Assert.IsNotNull(attachments);
            Assert.AreEqual(attachments[0].Id, _stubFileDto.Id);
            Assert.AreEqual(attachments[0].Name, _stubFileDto.Name);
            Assert.AreEqual(attachments[0].Size, _stubFileDto.Size);
            Assert.AreEqual(attachments[0].Status, _stubFileDto.Status);
            Assert.AreEqual(attachments[0].UploadedBy, _stubFileDto.UploadedBy);
            Assert.AreEqual(attachments[0].UploadedOn, _stubFileDto.UploadedOn);
            Assert.AreEqual(attachments[0].UploaderId, _stubFileDto.UploaderId);
        }

        [Test]
        public async Task GetAttachments_NoAttachments_GracefulHandle()
        {
            // Arrange
            var itemId = Guid.NewGuid();

            // Act
            var attachments = await _attachmentsRepository.GetAttachments(itemId);

            // Assert
            Assert.IsNotNull(attachments);
            Assert.IsEmpty(attachments);
        }

        [Test]
        public async Task UpdateAttachmentStatus_Valid_Success()
        {
            // Arrange
            var update = _stubStatusUpdate;

            // Act
            var success = await _attachmentsRepository.UpdateAttachmentStatus(update);

            // Assert
            Assert.IsNotNull(success);
            Assert.IsTrue(success);
        }
    }
}