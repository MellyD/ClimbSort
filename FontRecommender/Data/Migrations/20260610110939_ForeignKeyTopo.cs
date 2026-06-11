using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FontRecommender.Migrations
{
    /// <inheritdoc />
    public partial class ForeignKeyTopo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ClimbId",
                table: "Topography",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Topography_ClimbId",
                table: "Topography",
                column: "ClimbId");

            migrationBuilder.AddForeignKey(
                name: "FK_Topography_Climb_ClimbId",
                table: "Topography",
                column: "ClimbId",
                principalTable: "Climb",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);


            migrationBuilder.DropIndex(
                name: "IX_Climb_CragId1",
                table: "Climb");

            migrationBuilder.DropIndex(
                name: "IX_Climb_GradeId1",
                table: "Climb");

            migrationBuilder.DropForeignKey(
                name: "FK_Climb_Crag_CragId1",
                table: "Climb");

            migrationBuilder.DropForeignKey(
                name: "FK_Climb_Grade_GradeId1",
                table: "Climb");

            migrationBuilder.DropColumn(
                name: "CragId1",
                table: "Climb");

            migrationBuilder.DropColumn(
                name: "GradeId1",
                table: "Climb");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Topography_Climb_ClimbId",
                table: "Topography");

            migrationBuilder.DropIndex(
                name: "IX_Topography_ClimbId",
                table: "Topography");

            migrationBuilder.DropColumn(
                name: "ClimbId",
                table: "Topography");

            migrationBuilder.AddColumn<Guid>(
                name: "CragId1",
                table: "Climb",
                type: "uniqueidentifier",
                nullable: true,
                defaultValue: null);

            migrationBuilder.AddColumn<Guid>(
                name: "GradeId1",
                table: "Climb",
                type: "uniqueidentifier",
                nullable: true,
                defaultValue: null);

            migrationBuilder.CreateIndex(
                name: "IX_Climb_CragId1",
                table: "Climb",
                column: "CragId1");

            migrationBuilder.CreateIndex(
                name: "IX_Climb_GradeId1",
                table: "Climb",
                column: "GradeId1");

            migrationBuilder.AddForeignKey(
                        name: "FK_Climb_Grade_GradeId1",
                        table: "Climb",
                        column: "GradeId1",
                        principalTable: "Grade",
                        principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                        name: "FK_Climb_Crag_CragId1",
                        table: "Climb",
                        column: "CragId1",
                        principalTable: "Crag",
                        principalColumn: "Id");
        }
    }
}
