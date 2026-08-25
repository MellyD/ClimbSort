using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClimbSort.Migrations
{
    /// <inheritdoc />
    public partial class DangerousBool : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Dangerous",
                table: "Climb",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Dangerous",
                table: "Climb");
        }
    }
}
