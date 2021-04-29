using Microsoft.EntityFrameworkCore.Migrations;

namespace TDG.STS.IdentityServer.Migrations
{
    public partial class MasterData_FixTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MasterDataDocumentTypes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterDataDocumentTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MasterDataDocumentTypes_Name",
                table: "MasterDataDocumentTypes",
                column: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MasterDataDocumentTypes");
        }
    }
}
