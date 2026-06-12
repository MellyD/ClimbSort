using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FontRecommender.Migrations
{
    /// <inheritdoc />
    public partial class InitialBuild : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Crag",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SearchName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CountryCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Crag", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GradingSystem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Discipline = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GradingSystem", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WallType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WallType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Grade",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GradeLabel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GradingSystemId = table.Column<int>(type: "int", nullable: false),
                    ScaleOrder = table.Column<int>(type: "int", nullable: false),
                    MinDifficultyRank = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaxDifficultyRank = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MeanDifficultyRank = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grade", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Grade_GradingSystem_GradingSystemId",
                        column: x => x.GradingSystemId,
                        principalTable: "GradingSystem",
                        principalColumn: "Id");
                });

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

            migrationBuilder.CreateTable(
                name: "Climb",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GradeId = table.Column<int>(type: "int", nullable: true),
                    WallTypeId = table.Column<int>(type: "int", nullable: false),
                    Popularity = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SearchName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CragId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CircuitId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CircuitNumber = table.Column<int>(type: "int", nullable: true),
                    SitStart = table.Column<bool>(type: "bit", nullable: true),
                    Rating = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TopographyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Climb", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Climb_Circuit_CircuitId",
                        column: x => x.CircuitId,
                        principalTable: "Circuit",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Climb_Crag_CragId",
                        column: x => x.CragId,
                        principalTable: "Crag",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Climb_Grade_GradeId",
                        column: x => x.GradeId,
                        principalTable: "Grade",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Climb_WallType_WallTypeId",
                        column: x => x.WallTypeId,
                        principalTable: "WallType",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Tag",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TagType = table.Column<int>(type: "int", nullable: false),
                    CragId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ClimbId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "Topography",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileReference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClimbId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Topography", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Topography_Climb_ClimbId",
                        column: x => x.ClimbId,
                        principalTable: "Climb",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Coordinates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClimbId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CragId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TopographyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CircuitId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CoordinateType = table.Column<int>(type: "int", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coordinates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Coordinates_Circuit_CircuitId",
                        column: x => x.CircuitId,
                        principalTable: "Circuit",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Coordinates_Climb_ClimbId",
                        column: x => x.ClimbId,
                        principalTable: "Climb",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Coordinates_Crag_CragId",
                        column: x => x.CragId,
                        principalTable: "Crag",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Coordinates_Topography_TopographyId",
                        column: x => x.TopographyId,
                        principalTable: "Topography",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Circuit_GradeId",
                table: "Circuit",
                column: "GradeId");

            migrationBuilder.CreateIndex(
                name: "IX_Circuit_ModifiedDate",
                table: "Circuit",
                column: "ModifiedDate",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_Climb_CircuitId",
                table: "Climb",
                column: "CircuitId");

            migrationBuilder.CreateIndex(
                name: "IX_Climb_CragId",
                table: "Climb",
                column: "CragId");

            migrationBuilder.CreateIndex(
                name: "IX_Climb_GradeId",
                table: "Climb",
                column: "GradeId");

            migrationBuilder.CreateIndex(
                name: "IX_Climb_ModifiedDate",
                table: "Climb",
                column: "ModifiedDate",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_Climb_TopographyId",
                table: "Climb",
                column: "TopographyId");

            migrationBuilder.CreateIndex(
                name: "IX_Climb_WallTypeId",
                table: "Climb",
                column: "WallTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Coordinates_CircuitId",
                table: "Coordinates",
                column: "CircuitId");

            migrationBuilder.CreateIndex(
                name: "IX_Coordinates_ClimbId",
                table: "Coordinates",
                column: "ClimbId");

            migrationBuilder.CreateIndex(
                name: "IX_Coordinates_CragId",
                table: "Coordinates",
                column: "CragId");

            migrationBuilder.CreateIndex(
                name: "IX_Coordinates_TopographyId",
                table: "Coordinates",
                column: "TopographyId");

            migrationBuilder.CreateIndex(
                name: "IX_Crag_ModifiedDate",
                table: "Crag",
                column: "ModifiedDate",
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_Grade_GradingSystemId",
                table: "Grade",
                column: "GradingSystemId");

            migrationBuilder.CreateIndex(
                name: "IX_Grade_ScaleOrder",
                table: "Grade",
                column: "ScaleOrder");

            migrationBuilder.CreateIndex(
                name: "IX_Tag_ClimbId",
                table: "Tag",
                column: "ClimbId");

            migrationBuilder.CreateIndex(
                name: "IX_Tag_CragId",
                table: "Tag",
                column: "CragId");

            migrationBuilder.CreateIndex(
                name: "IX_Topography_ClimbId",
                table: "Topography",
                column: "ClimbId");

            migrationBuilder.CreateIndex(
                name: "IX_Topography_ModifiedDate",
                table: "Topography",
                column: "ModifiedDate",
                descending: new bool[0]);

            migrationBuilder.AddForeignKey(
                name: "FK_Climb_Topography_TopographyId",
                table: "Climb",
                column: "TopographyId",
                principalTable: "Topography",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Circuit_Grade_GradeId",
                table: "Circuit");

            migrationBuilder.DropForeignKey(
                name: "FK_Climb_Grade_GradeId",
                table: "Climb");

            migrationBuilder.DropForeignKey(
                name: "FK_Climb_Circuit_CircuitId",
                table: "Climb");

            migrationBuilder.DropForeignKey(
                name: "FK_Climb_Crag_CragId",
                table: "Climb");

            migrationBuilder.DropForeignKey(
                name: "FK_Climb_Topography_TopographyId",
                table: "Climb");

            migrationBuilder.DropTable(
                name: "Coordinates");

            migrationBuilder.DropTable(
                name: "Tag");

            migrationBuilder.DropTable(
                name: "Grade");

            migrationBuilder.DropTable(
                name: "GradingSystem");

            migrationBuilder.DropTable(
                name: "Circuit");

            migrationBuilder.DropTable(
                name: "Crag");

            migrationBuilder.DropTable(
                name: "Topography");

            migrationBuilder.DropTable(
                name: "Climb");

            migrationBuilder.DropTable(
                name: "WallType");
        }
    }
}
