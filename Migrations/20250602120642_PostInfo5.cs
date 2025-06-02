using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Utterly.Migrations
{
    /// <inheritdoc />
    public partial class PostInfo5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParentPostId",
                table: "UtterlyPosts",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParentPostId",
                table: "UtterlyPosts");
        }
    }
}
