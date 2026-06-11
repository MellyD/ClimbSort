using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FontRecommender.Migrations
{
    /// <inheritdoc />
    public partial class CircuitNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CircuitNumber",
                table: "Climb",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CircuitNumber",
                table: "Climb");
        }
    }
}
