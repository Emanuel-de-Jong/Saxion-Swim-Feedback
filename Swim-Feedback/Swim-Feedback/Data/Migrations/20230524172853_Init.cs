using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Swim_Feedback.Data.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.CreateTable(
                name: "StudentImages",
                columns: table => new
                {
                    StudentImageId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Data = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_StudentImages", x => x.StudentImageId);
                });

            _ = migrationBuilder.CreateTable(
                name: "SwimClasses",
                columns: table => new
                {
                    SwimClassId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_SwimClasses", x => x.SwimClassId);
                });

            _ = migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    StudentId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentImageId = table.Column<long>(type: "bigint", nullable: true),
                    SwimClassId = table.Column<long>(type: "bigint", nullable: true),
                    CustomerNumber = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Points = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_Students", x => x.StudentId);
                    _ = table.ForeignKey(
                        name: "FK_Students_StudentImages_StudentImageId",
                        column: x => x.StudentImageId,
                        principalTable: "StudentImages",
                        principalColumn: "StudentImageId");
                    _ = table.ForeignKey(
                        name: "FK_Students_SwimClasses_SwimClassId",
                        column: x => x.SwimClassId,
                        principalTable: "SwimClasses",
                        principalColumn: "SwimClassId");
                });

            _ = migrationBuilder.CreateTable(
                name: "StudentFeedbacks",
                columns: table => new
                {
                    StudentFeedbackId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<long>(type: "bigint", nullable: true),
                    Topic = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Score = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_StudentFeedbacks", x => x.StudentFeedbackId);
                    _ = table.ForeignKey(
                        name: "FK_StudentFeedbacks_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "StudentId");
                });

            _ = migrationBuilder.CreateTable(
                name: "TeacherFeedbacks",
                columns: table => new
                {
                    TeacherFeedbackId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    SwimClassId = table.Column<long>(type: "bigint", nullable: true),
                    StudentId = table.Column<long>(type: "bigint", nullable: true),
                    Topic = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    _ = table.PrimaryKey("PK_TeacherFeedbacks", x => x.TeacherFeedbackId);
                    _ = table.ForeignKey(
                        name: "FK_TeacherFeedbacks_AspNetUsers_AccountId",
                        column: x => x.AccountId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    _ = table.ForeignKey(
                        name: "FK_TeacherFeedbacks_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "StudentId");
                    _ = table.ForeignKey(
                        name: "FK_TeacherFeedbacks_SwimClasses_SwimClassId",
                        column: x => x.SwimClassId,
                        principalTable: "SwimClasses",
                        principalColumn: "SwimClassId");
                });

            _ = migrationBuilder.CreateIndex(
                name: "IX_StudentFeedbacks_StudentId",
                table: "StudentFeedbacks",
                column: "StudentId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_Students_StudentImageId",
                table: "Students",
                column: "StudentImageId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_Students_SwimClassId",
                table: "Students",
                column: "SwimClassId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_TeacherFeedbacks_AccountId",
                table: "TeacherFeedbacks",
                column: "AccountId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_TeacherFeedbacks_StudentId",
                table: "TeacherFeedbacks",
                column: "StudentId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_TeacherFeedbacks_SwimClassId",
                table: "TeacherFeedbacks",
                column: "SwimClassId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropTable(
                name: "StudentFeedbacks");

            _ = migrationBuilder.DropTable(
                name: "TeacherFeedbacks");

            _ = migrationBuilder.DropTable(
                name: "Students");

            _ = migrationBuilder.DropTable(
                name: "StudentImages");

            _ = migrationBuilder.DropTable(
                name: "SwimClasses");
        }
    }
}
