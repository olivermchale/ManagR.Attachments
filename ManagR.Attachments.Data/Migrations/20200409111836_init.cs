using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ManagR.Attachments.Data.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Attachment",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UploadedOn = table.Column<Guid>(nullable: false),
                    UploaderId = table.Column<Guid>(nullable: false),
                    UploadedBy = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    LastModifiedDate = table.Column<DateTime>(nullable: false),
                    Size = table.Column<int>(nullable: false),
                    Type = table.Column<string>(nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachment", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attachment");
        }
    }
}
