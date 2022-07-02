using Microsoft.EntityFrameworkCore.Migrations;

namespace FrontToBack.Migrations
{
    public partial class RenameTableBiotoBios : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Bio",
                table: "Bio");

            migrationBuilder.RenameTable(
                name: "Bio",
                newName: "Bios");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Bios",
                table: "Bios",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Bios",
                table: "Bios");

            migrationBuilder.RenameTable(
                name: "Bios",
                newName: "Bio");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Bio",
                table: "Bio",
                column: "Id");
        }
    }
}
