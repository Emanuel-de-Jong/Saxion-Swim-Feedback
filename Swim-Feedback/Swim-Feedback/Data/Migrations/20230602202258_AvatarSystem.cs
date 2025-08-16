using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Swim_Feedback.Data.Migrations
{
    public partial class AvatarSystem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropTable(
                name: "StudentAccessories");

            _ = migrationBuilder.AddColumn<long>(
                name: "AvatarId",
                table: "Students",
                type: "bigint",
                nullable: true);

            _ = migrationBuilder.AddColumn<int>(
                name: "AccessoryType",
                table: "Accessories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            _ = migrationBuilder.CreateTable(
                name: "BodyParts",
                columns: table => new
                {
                    BodyPartId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BodyPartType = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_BodyParts", x => x.BodyPartId);
                });

            _ = migrationBuilder.CreateTable(
                name: "Avatars",
                columns: table => new
                {
                    AvatarId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SkinId = table.Column<long>(type: "bigint", nullable: false),
                    FaceFormId = table.Column<long>(type: "bigint", nullable: false),
                    HairId = table.Column<long>(type: "bigint", nullable: false),
                    EyesId = table.Column<long>(type: "bigint", nullable: false),
                    MouthId = table.Column<long>(type: "bigint", nullable: false),
                    BackgroundAccessoryId = table.Column<long>(type: "bigint", nullable: false),
                    HairAccessoryId = table.Column<long>(type: "bigint", nullable: true),
                    EyesAccessoryId = table.Column<long>(type: "bigint", nullable: true),
                    MouthAccessoryId = table.Column<long>(type: "bigint", nullable: true),
                    NeckAccessoryId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_Avatars", x => x.AvatarId);
                    _ = table.ForeignKey(
                        name: "FK_Avatars_Accessories_BackgroundAccessoryId",
                        column: x => x.BackgroundAccessoryId,
                        principalTable: "Accessories",
                        principalColumn: "AccessoryId");
                    _ = table.ForeignKey(
                        name: "FK_Avatars_Accessories_EyesAccessoryId",
                        column: x => x.EyesAccessoryId,
                        principalTable: "Accessories",
                        principalColumn: "AccessoryId");
                    _ = table.ForeignKey(
                        name: "FK_Avatars_Accessories_HairAccessoryId",
                        column: x => x.HairAccessoryId,
                        principalTable: "Accessories",
                        principalColumn: "AccessoryId");
                    _ = table.ForeignKey(
                        name: "FK_Avatars_Accessories_MouthAccessoryId",
                        column: x => x.MouthAccessoryId,
                        principalTable: "Accessories",
                        principalColumn: "AccessoryId");
                    _ = table.ForeignKey(
                        name: "FK_Avatars_Accessories_NeckAccessoryId",
                        column: x => x.NeckAccessoryId,
                        principalTable: "Accessories",
                        principalColumn: "AccessoryId");
                    _ = table.ForeignKey(
                        name: "FK_Avatars_BodyParts_EyesId",
                        column: x => x.EyesId,
                        principalTable: "BodyParts",
                        principalColumn: "BodyPartId");
                    _ = table.ForeignKey(
                        name: "FK_Avatars_BodyParts_FaceFormId",
                        column: x => x.FaceFormId,
                        principalTable: "BodyParts",
                        principalColumn: "BodyPartId");
                    _ = table.ForeignKey(
                        name: "FK_Avatars_BodyParts_HairId",
                        column: x => x.HairId,
                        principalTable: "BodyParts",
                        principalColumn: "BodyPartId");
                    _ = table.ForeignKey(
                        name: "FK_Avatars_BodyParts_MouthId",
                        column: x => x.MouthId,
                        principalTable: "BodyParts",
                        principalColumn: "BodyPartId");
                    _ = table.ForeignKey(
                        name: "FK_Avatars_BodyParts_SkinId",
                        column: x => x.SkinId,
                        principalTable: "BodyParts",
                        principalColumn: "BodyPartId");
                });

            _ = migrationBuilder.CreateTable(
                name: "AvatarAccessories",
                columns: table => new
                {
                    AvatarId = table.Column<long>(type: "bigint", nullable: false),
                    AccessoryId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_AvatarAccessories", x => new { x.AvatarId, x.AccessoryId });
                    _ = table.ForeignKey(
                        name: "FK_AvatarAccessories_Accessories_AccessoryId",
                        column: x => x.AccessoryId,
                        principalTable: "Accessories",
                        principalColumn: "AccessoryId",
                        onDelete: ReferentialAction.Cascade);
                    _ = table.ForeignKey(
                        name: "FK_AvatarAccessories_Avatars_AvatarId",
                        column: x => x.AvatarId,
                        principalTable: "Avatars",
                        principalColumn: "AvatarId",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "IX_Students_AvatarId",
                table: "Students",
                column: "AvatarId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_AvatarAccessories_AccessoryId",
                table: "AvatarAccessories",
                column: "AccessoryId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_AvatarBodyParts_BodyPartId",
                table: "AvatarBodyParts",
                column: "BodyPartId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_Avatars_BackgroundAccessoryId",
                table: "Avatars",
                column: "BackgroundAccessoryId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_Avatars_EyesAccessoryId",
                table: "Avatars",
                column: "EyesAccessoryId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_Avatars_EyesId",
                table: "Avatars",
                column: "EyesId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_Avatars_FaceFormId",
                table: "Avatars",
                column: "FaceFormId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_Avatars_HairAccessoryId",
                table: "Avatars",
                column: "HairAccessoryId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_Avatars_HairId",
                table: "Avatars",
                column: "HairId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_Avatars_MouthAccessoryId",
                table: "Avatars",
                column: "MouthAccessoryId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_Avatars_MouthId",
                table: "Avatars",
                column: "MouthId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_Avatars_NeckAccessoryId",
                table: "Avatars",
                column: "NeckAccessoryId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_Avatars_SkinId",
                table: "Avatars",
                column: "SkinId");

            _ = migrationBuilder.AddForeignKey(
                name: "FK_Students_Avatars_AvatarId",
                table: "Students",
                column: "AvatarId",
                principalTable: "Avatars",
                principalColumn: "AvatarId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropForeignKey(
                name: "FK_Students_Avatars_AvatarId",
                table: "Students");

            _ = migrationBuilder.DropTable(
                name: "AvatarAccessories");

            _ = migrationBuilder.DropTable(
                name: "AvatarBodyParts");

            _ = migrationBuilder.DropTable(
                name: "Avatars");

            _ = migrationBuilder.DropTable(
                name: "BodyParts");

            _ = migrationBuilder.DropIndex(
                name: "IX_Students_AvatarId",
                table: "Students");

            _ = migrationBuilder.DropColumn(
                name: "AvatarId",
                table: "Students");

            _ = migrationBuilder.DropColumn(
                name: "AccessoryType",
                table: "Accessories");

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
    }
}
