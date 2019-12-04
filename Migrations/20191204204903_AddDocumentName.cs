using Microsoft.EntityFrameworkCore.Migrations;

namespace BetterDocs.Migrations
{
    public partial class AddDocumentName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TextDocuments",
                keyColumn: "Id",
                keyValue: "1");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "TextDocuments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "TextDocuments");

            migrationBuilder.InsertData(
                table: "TextDocuments",
                columns: new[] { "Id", "ContributorId", "OwnerId", "Text" },
                values: new object[] { "1", "7993615d-93bf-4e07-9b4f-a193e55cf1a9", "7993615d-93bf-4e07-9b4f-a193e55cf1a9", "empty" });
        }
    }
}
