using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Un2Trek.Trekis.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCaptureTypeToTreki : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CaptureType",
                table: "Trekis",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CaptureType",
                table: "Trekis");
        }
    }
}
