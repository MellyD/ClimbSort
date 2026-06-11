using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FontRecommender.Migrations
{
    /// <inheritdoc />
    public partial class ClimbSearchName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SearchName",
                table: "Climb",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SearchName",
                table: "Climb");
        }
    }
}
