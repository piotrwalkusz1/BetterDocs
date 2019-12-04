using Microsoft.EntityFrameworkCore.Migrations;

namespace BetterDocs.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TextDocuments",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    OwnerId = table.Column<string>(nullable: true),
                    ContributorId = table.Column<string>(nullable: true),
                    Text = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TextDocuments", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "TextDocuments",
                columns: new[] { "Id", "ContributorId", "OwnerId", "Text" },
                values: new object[] { "1", "7993615d-93bf-4e07-9b4f-a193e55cf1a9", "7993615d-93bf-4e07-9b4f-a193e55cf1a9", "empty" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TextDocuments");
        }
    }
}
