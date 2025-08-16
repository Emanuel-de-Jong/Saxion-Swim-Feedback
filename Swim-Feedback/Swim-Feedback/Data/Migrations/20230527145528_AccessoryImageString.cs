using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Swim_Feedback.Data.Migrations
{
    public partial class AccessoryImageString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropColumn(
                name: "Data",
                table: "Accessories");

            _ = migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Accessories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropColumn(
                name: "Image",
                table: "Accessories");

            _ = migrationBuilder.AddColumn<byte[]>(
                name: "Data",
                table: "Accessories",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }
    }
}
