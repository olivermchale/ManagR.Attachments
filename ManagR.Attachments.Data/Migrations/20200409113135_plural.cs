using Microsoft.EntityFrameworkCore.Migrations;

namespace ManagR.Attachments.Data.Migrations
{
    public partial class plural : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Attachment",
                table: "Attachment");

            migrationBuilder.RenameTable(
                name: "Attachment",
                newName: "Attachments");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Attachments",
                table: "Attachments",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Attachments",
                table: "Attachments");

            migrationBuilder.RenameTable(
                name: "Attachments",
                newName: "Attachment");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Attachment",
                table: "Attachment",
                column: "Id");
        }
    }
}
