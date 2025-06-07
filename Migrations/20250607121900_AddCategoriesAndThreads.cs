using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Utterly.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoriesAndThreads : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ThreadId",
                table: "UtterlyPosts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Threads",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Threads", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Threads_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Threads_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UtterlyPosts_ParentPostId",
                table: "UtterlyPosts",
                column: "ParentPostId");

            migrationBuilder.CreateIndex(
                name: "IX_UtterlyPosts_ThreadId",
                table: "UtterlyPosts",
                column: "ThreadId");

            migrationBuilder.CreateIndex(
                name: "IX_Threads_CategoryId",
                table: "Threads",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Threads_UserId",
                table: "Threads",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UtterlyPosts_Threads_ThreadId",
                table: "UtterlyPosts",
                column: "ThreadId",
                principalTable: "Threads",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UtterlyPosts_UtterlyPosts_ParentPostId",
                table: "UtterlyPosts",
                column: "ParentPostId",
                principalTable: "UtterlyPosts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UtterlyPosts_Threads_ThreadId",
                table: "UtterlyPosts");

            migrationBuilder.DropForeignKey(
                name: "FK_UtterlyPosts_UtterlyPosts_ParentPostId",
                table: "UtterlyPosts");

            migrationBuilder.DropTable(
                name: "Threads");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_UtterlyPosts_ParentPostId",
                table: "UtterlyPosts");

            migrationBuilder.DropIndex(
                name: "IX_UtterlyPosts_ThreadId",
                table: "UtterlyPosts");

            migrationBuilder.DropColumn(
                name: "ThreadId",
                table: "UtterlyPosts");
        }
    }
}
