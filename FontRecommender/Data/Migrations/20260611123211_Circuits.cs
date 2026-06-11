using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FontRecommender.Migrations
{
    /// <inheritdoc />
    public partial class Circuits : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CircuitId",
                table: "Coordinates",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CircuitId",
                table: "Climb",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Circuit",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Colour = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GradeId = table.Column<int>(type: "int", nullable: true),
                    Beginner = table.Column<bool>(type: "bit", nullable: true),
                    Dangerous = table.Column<bool>(type: "bit", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Circuit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Circuit_Grade_GradeId",
                        column: x => x.GradeId,
                        principalTable: "Grade",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Coordinates_CircuitId",
                table: "Coordinates",
                column: "CircuitId");

            migrationBuilder.CreateIndex(
                name: "IX_Climb_CircuitId",
                table: "Climb",
                column: "CircuitId");

            migrationBuilder.CreateIndex(
                name: "IX_Circuit_GradeId",
                table: "Circuit",
                column: "GradeId");

            migrationBuilder.CreateIndex(
                name: "IX_Circuit_ModifiedDate",
                table: "Circuit",
                column: "ModifiedDate",
                descending: new bool[0]);

            migrationBuilder.AddForeignKey(
                name: "FK_Climb_Circuit_CircuitId",
                table: "Climb",
                column: "CircuitId",
                principalTable: "Circuit",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Coordinates_Circuit_CircuitId",
                table: "Coordinates",
                column: "CircuitId",
                principalTable: "Circuit",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Climb_Circuit_CircuitId",
                table: "Climb");

            migrationBuilder.DropForeignKey(
                name: "FK_Coordinates_Circuit_CircuitId",
                table: "Coordinates");

            migrationBuilder.DropTable(
                name: "Circuit");

            migrationBuilder.DropIndex(
                name: "IX_Coordinates_CircuitId",
                table: "Coordinates");

            migrationBuilder.DropIndex(
                name: "IX_Climb_CircuitId",
                table: "Climb");

            migrationBuilder.DropColumn(
                name: "CircuitId",
                table: "Coordinates");

            migrationBuilder.DropColumn(
                name: "CircuitId",
                table: "Climb");
        }
    }
}
