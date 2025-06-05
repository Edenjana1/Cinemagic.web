using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinemagic.Migrations
{
    /// <inheritdoc />
    public partial class AddOptionsSerializedToQuestions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OptionsSerialized",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OptionsSerialized",
                table: "Questions");
        }
    }
}
