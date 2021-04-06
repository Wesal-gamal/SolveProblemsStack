using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLayer.Migrations
{
    public partial class updateemployee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Branches",
                table: "Branches");

            migrationBuilder.DropColumn(
                name: "branchId",
                table: "Branches");

            migrationBuilder.RenameColumn(
                name: "nameEn",
                table: "Branches",
                newName: "NameEn");

            migrationBuilder.RenameColumn(
                name: "nameAr",
                table: "Branches",
                newName: "NameAr");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Branches",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Branches",
                table: "Branches",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_branchId",
                table: "Employees",
                column: "branchId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Branches_branchId",
                table: "Employees",
                column: "branchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Branches_branchId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_branchId",
                table: "Employees");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Branches",
                table: "Branches");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Branches");

            migrationBuilder.RenameColumn(
                name: "NameEn",
                table: "Branches",
                newName: "nameEn");

            migrationBuilder.RenameColumn(
                name: "NameAr",
                table: "Branches",
                newName: "nameAr");

            migrationBuilder.AddColumn<int>(
                name: "branchId",
                table: "Branches",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Branches",
                table: "Branches",
                column: "branchId");
        }
    }
}
