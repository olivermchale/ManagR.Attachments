using ManagR.Attachments.Models.Dtos;
using Microsoft.EntityFrameworkCore;
using System;

namespace ManagR.Attachments.Data
{
    public class AttachmentsDb : DbContext
    {
        public DbSet<FileDto> Attachments { get; set; }

        public AttachmentsDb(DbContextOptions<AttachmentsDb> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<FileDto>(b =>
            {
                b.Property(p => p.Id).IsRequired();
                b.Property(p => p.LastModifiedDate).IsRequired();
                b.Property(p => p.Name).IsRequired();
                b.Property(p => p.Size).IsRequired();
                b.Property(p => p.Status).IsRequired();
                b.Property(p => p.Type).IsRequired();
                b.Property(p => p.UploadedBy).IsRequired();
                b.Property(p => p.UploadedOn).IsRequired();
                b.Property(p => p.UploaderId).IsRequired();
            });
        }

    }
}
