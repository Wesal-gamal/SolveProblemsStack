using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLayer.Migrations
{
    public partial class addsolutions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Problem_Problem_Id",
                table: "Comment");

            migrationBuilder.DropIndex(
                name: "IX_Comment_Problem_Id",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "Problem_Id",
                table: "Comment");

            migrationBuilder.AddColumn<int>(
                name: "SolutionsId",
                table: "CommentLike",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Solution_Id",
                table: "Comment",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Solutions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Problem_Id = table.Column<int>(nullable: false),
                    User_Id = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Solutions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Solutions_Problem_Problem_Id",
                        column: x => x.Problem_Id,
                        principalTable: "Problem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Solutions_AspNetUsers_User_Id",
                        column: x => x.User_Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommentLike_SolutionsId",
                table: "CommentLike",
                column: "SolutionsId");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_Solution_Id",
                table: "Comment",
                column: "Solution_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Solutions_Problem_Id",
                table: "Solutions",
                column: "Problem_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Solutions_User_Id",
                table: "Solutions",
                column: "User_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Solutions_Solution_Id",
                table: "Comment",
                column: "Solution_Id",
                principalTable: "Solutions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CommentLike_Solutions_SolutionsId",
                table: "CommentLike",
                column: "SolutionsId",
                principalTable: "Solutions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Solutions_Solution_Id",
                table: "Comment");

            migrationBuilder.DropForeignKey(
                name: "FK_CommentLike_Solutions_SolutionsId",
                table: "CommentLike");

            migrationBuilder.DropTable(
                name: "Solutions");

            migrationBuilder.DropIndex(
                name: "IX_CommentLike_SolutionsId",
                table: "CommentLike");

            migrationBuilder.DropIndex(
                name: "IX_Comment_Solution_Id",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "SolutionsId",
                table: "CommentLike");

            migrationBuilder.DropColumn(
                name: "Solution_Id",
                table: "Comment");

            migrationBuilder.AddColumn<int>(
                name: "Problem_Id",
                table: "Comment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Comment_Problem_Id",
                table: "Comment",
                column: "Problem_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Problem_Problem_Id",
                table: "Comment",
                column: "Problem_Id",
                principalTable: "Problem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
