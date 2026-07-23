using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FontRecommender.Migrations
{
    /// <inheritdoc />
    public partial class TagTypeAddition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Dangerous",
                table: "Climb");

            migrationBuilder.RenameColumn(
                name: "TagType",
                table: "Tag",
                newName: "TagTypeId");

            migrationBuilder.CreateTable(
                name: "TagType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagType", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tag_TagTypeId",
                table: "Tag",
                column: "TagTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tag_TagType_TagTypeId",
                table: "Tag",
                column: "TagTypeId",
                principalTable: "TagType",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tag_TagType_TagTypeId",
                table: "Tag");

            migrationBuilder.DropTable(
                name: "TagType");

            migrationBuilder.DropIndex(
                name: "IX_Tag_TagTypeId",
                table: "Tag");

            migrationBuilder.RenameColumn(
                name: "TagTypeId",
                table: "Tag",
                newName: "TagType");

            migrationBuilder.AddColumn<bool>(
                name: "Dangerous",
                table: "Climb",
                type: "bit",
                nullable: true);
        }
    }
}
