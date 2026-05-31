using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FontRecommender.Migrations
{
    /// <inheritdoc />
    public partial class InitialStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Coordinates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CoordinateType = table.Column<int>(type: "int", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coordinates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Crag",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    CountryCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GradingSystem", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Topography",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileReference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Topography", x => x.Id);
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
                name: "CoordinatesCrag",
                columns: table => new
                {
                    CoordinatesId = table.Column<int>(type: "int", nullable: false),
                    CragId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoordinatesCrag", x => new { x.CoordinatesId, x.CragId });
                    table.ForeignKey(
                        name: "FK_CoordinatesCrag_Coordinates_CoordinatesId",
                        column: x => x.CoordinatesId,
                        principalTable: "Coordinates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CoordinatesCrag_Crag_CragId",
                        column: x => x.CragId,
                        principalTable: "Crag",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Grade",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GradeLabel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GradingSystemId = table.Column<int>(type: "int", nullable: false),
                    Discipline = table.Column<int>(type: "int", nullable: false),
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
                name: "CoordinatesTopography",
                columns: table => new
                {
                    CoordinatesId = table.Column<int>(type: "int", nullable: false),
                    TopographyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoordinatesTopography", x => new { x.CoordinatesId, x.TopographyId });
                    table.ForeignKey(
                        name: "FK_CoordinatesTopography_Coordinates_CoordinatesId",
                        column: x => x.CoordinatesId,
                        principalTable: "Coordinates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CoordinatesTopography_Topography_TopographyId",
                        column: x => x.TopographyId,
                        principalTable: "Topography",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Climb",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    GradeId = table.Column<int>(type: "int", nullable: true),
                    WallTypeId = table.Column<int>(type: "int", nullable: false),
                    Popularity = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CragId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Rating = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CoordinatesId = table.Column<int>(type: "int", nullable: true),
                    TopographyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Climb", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Climb_Coordinates_CoordinatesId",
                        column: x => x.CoordinatesId,
                        principalTable: "Coordinates",
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
                        name: "FK_Climb_Topography_TopographyId",
                        column: x => x.TopographyId,
                        principalTable: "Topography",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Climb_WallType_WallTypeId",
                        column: x => x.WallTypeId,
                        principalTable: "WallType",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Climb_CoordinatesId",
                table: "Climb",
                column: "CoordinatesId");

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
                name: "IX_CoordinatesCrag_CragId",
                table: "CoordinatesCrag",
                column: "CragId");

            migrationBuilder.CreateIndex(
                name: "IX_CoordinatesTopography_TopographyId",
                table: "CoordinatesTopography",
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
                name: "IX_Topography_ModifiedDate",
                table: "Topography",
                column: "ModifiedDate",
                descending: new bool[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Climb");

            migrationBuilder.DropTable(
                name: "CoordinatesCrag");

            migrationBuilder.DropTable(
                name: "CoordinatesTopography");

            migrationBuilder.DropTable(
                name: "Grade");

            migrationBuilder.DropTable(
                name: "WallType");

            migrationBuilder.DropTable(
                name: "Crag");

            migrationBuilder.DropTable(
                name: "Coordinates");

            migrationBuilder.DropTable(
                name: "Topography");

            migrationBuilder.DropTable(
                name: "GradingSystem");
        }
    }
}
