using Microsoft.EntityFrameworkCore.Migrations;

namespace DataLayer.Migrations
{
    public partial class addSolutionLikes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Dislike",
                table: "CommentLike",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Like",
                table: "CommentLike",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "SolutionLike",
                columns: table => new
                {
                    Solution_Id = table.Column<int>(nullable: false),
                    User_Id = table.Column<string>(nullable: false),
                    Like = table.Column<bool>(nullable: false),
                    Dislike = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolutionLike", x => new { x.Solution_Id, x.User_Id });
                    table.ForeignKey(
                        name: "FK_SolutionLike_Solutions_Solution_Id",
                        column: x => x.Solution_Id,
                        principalTable: "Solutions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SolutionLike_AspNetUsers_User_Id",
                        column: x => x.User_Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SolutionLike_User_Id",
                table: "SolutionLike",
                column: "User_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SolutionLike");

            migrationBuilder.DropColumn(
                name: "Dislike",
                table: "CommentLike");

            migrationBuilder.DropColumn(
                name: "Like",
                table: "CommentLike");
        }
    }
}
