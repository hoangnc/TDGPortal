using Microsoft.EntityFrameworkCore.Migrations;

namespace TDG.STS.IdentityServer.Migrations
{
    public partial class MasterData_Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MasterDataCompanies",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    EmailAddress = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterDataCompanies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MasterDataDepartments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    EmailAddress = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterDataDepartments", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MasterDataCompanies_Name",
                table: "MasterDataCompanies",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_MasterDataDepartments_Name",
                table: "MasterDataDepartments",
                column: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MasterDataCompanies");

            migrationBuilder.DropTable(
                name: "MasterDataDepartments");
        }
    }
}
