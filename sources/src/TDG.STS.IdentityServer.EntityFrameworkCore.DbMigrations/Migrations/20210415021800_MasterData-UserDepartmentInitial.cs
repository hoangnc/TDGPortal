using Microsoft.EntityFrameworkCore.Migrations;

namespace TDG.STS.IdentityServer.Migrations
{
    public partial class MasterDataUserDepartmentInitial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MasterDataUserDepartments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DepartmentCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterDataUserDepartments", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MasterDataUserDepartments_UserName_DepartmentCode",
                table: "MasterDataUserDepartments",
                columns: new[] { "UserName", "DepartmentCode" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MasterDataUserDepartments");
        }
    }
}
