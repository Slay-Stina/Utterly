using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Utterly.Migrations
{
    /// <inheritdoc />
    public partial class PostInfo2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UtterlyPosts_AspNetUsers_UserId",
                table: "UtterlyPosts");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "UtterlyPosts",
                newName: "UserName");

            migrationBuilder.RenameIndex(
                name: "IX_UtterlyPosts_UserId",
                table: "UtterlyPosts",
                newName: "IX_UtterlyPosts_UserName");

            migrationBuilder.AddForeignKey(
                name: "FK_UtterlyPosts_AspNetUsers_UserName",
                table: "UtterlyPosts",
                column: "UserName",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UtterlyPosts_AspNetUsers_UserName",
                table: "UtterlyPosts");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "UtterlyPosts",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UtterlyPosts_UserName",
                table: "UtterlyPosts",
                newName: "IX_UtterlyPosts_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UtterlyPosts_AspNetUsers_UserId",
                table: "UtterlyPosts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
