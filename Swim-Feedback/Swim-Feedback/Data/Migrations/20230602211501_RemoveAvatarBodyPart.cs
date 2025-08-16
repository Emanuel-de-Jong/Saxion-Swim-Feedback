using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Swim_Feedback.Data.Migrations
{
    public partial class RemoveAvatarBodyPart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropTable(
                name: "AvatarBodyParts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.CreateTable(
                name: "AvatarBodyParts",
                columns: table => new
                {
                    AvatarId = table.Column<long>(type: "bigint", nullable: false),
                    BodyPartId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_AvatarBodyParts", x => new { x.AvatarId, x.BodyPartId });
                    _ = table.ForeignKey(
                        name: "FK_AvatarBodyParts_Avatars_AvatarId",
                        column: x => x.AvatarId,
                        principalTable: "Avatars",
                        principalColumn: "AvatarId",
                        onDelete: ReferentialAction.Cascade);
                    _ = table.ForeignKey(
                        name: "FK_AvatarBodyParts_BodyParts_BodyPartId",
                        column: x => x.BodyPartId,
                        principalTable: "BodyParts",
                        principalColumn: "BodyPartId",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateIndex(
                name: "IX_AvatarBodyParts_BodyPartId",
                table: "AvatarBodyParts",
                column: "BodyPartId");
        }
    }
}
