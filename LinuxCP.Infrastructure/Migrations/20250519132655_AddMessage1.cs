using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LinuxCP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMessage1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_chatMessages_Users_UserId",
                table: "chatMessages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_chatMessages",
                table: "chatMessages");

            migrationBuilder.RenameTable(
                name: "chatMessages",
                newName: "СhatMessages");

            migrationBuilder.RenameIndex(
                name: "IX_chatMessages_UserId",
                table: "СhatMessages",
                newName: "IX_СhatMessages_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_СhatMessages",
                table: "СhatMessages",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_СhatMessages_Users_UserId",
                table: "СhatMessages",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_СhatMessages_Users_UserId",
                table: "СhatMessages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_СhatMessages",
                table: "СhatMessages");

            migrationBuilder.RenameTable(
                name: "СhatMessages",
                newName: "chatMessages");

            migrationBuilder.RenameIndex(
                name: "IX_СhatMessages_UserId",
                table: "chatMessages",
                newName: "IX_chatMessages_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_chatMessages",
                table: "chatMessages",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_chatMessages_Users_UserId",
                table: "chatMessages",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
