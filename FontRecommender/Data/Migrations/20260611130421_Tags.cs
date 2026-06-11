using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FontRecommender.Migrations
{
    /// <inheritdoc />
    public partial class Tags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Crag",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SearchName",
                table: "Crag",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "SitStart",
                table: "Climb",
                type: "bit",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Tag",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TagType = table.Column<int>(type: "int", nullable: false),
                    CragId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ClimbId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tag", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tag_Climb_ClimbId",
                        column: x => x.ClimbId,
                        principalTable: "Climb",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Tag_Crag_CragId",
                        column: x => x.CragId,
                        principalTable: "Crag",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tag_ClimbId",
                table: "Tag",
                column: "ClimbId");

            migrationBuilder.CreateIndex(
                name: "IX_Tag_CragId",
                table: "Tag",
                column: "CragId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tag");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Crag");

            migrationBuilder.DropColumn(
                name: "SearchName",
                table: "Crag");

            migrationBuilder.DropColumn(
                name: "SitStart",
                table: "Climb");
        }
    }
}
