using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Swim_Feedback.Data.Migrations
{
    public partial class StudentAccessoryTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.CreateTable(
                name: "StudentAccessories",
                columns: table => new
                {
                    StudentId = table.Column<long>(type: "bigint", nullable: false),
                    AccessoryId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_StudentAccessories", x => new { x.StudentId, x.AccessoryId });
                    _ = table.ForeignKey(
                        name: "FK_StudentAccessories_Accessories_AccessoryId",
                        column: x => x.AccessoryId,
                        principalTable: "Accessories",
                        principalColumn: "AccessoryId",
                        onDelete: ReferentialAction.Cascade);
                    _ = table.ForeignKey(
                        name: "FK_StudentAccessories_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "StudentId",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateIndex(
                name: "IX_StudentAccessories_AccessoryId",
                table: "StudentAccessories",
                column: "AccessoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropTable(
                name: "StudentAccessories");
        }
    }
}
