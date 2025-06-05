using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinemagic.Migrations
{
    /// <inheritdoc />
    public partial class Quizzes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Members_UserID",
                table: "Comments");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Members_UserID",
                table: "Comments",
                column: "UserID",
                principalTable: "Members",
                principalColumn: "MemberID",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Members_UserID",
                table: "Comments");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Members_UserID",
                table: "Comments",
                column: "UserID",
                principalTable: "Members",
                principalColumn: "MemberID");
        }
    }
}
