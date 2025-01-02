using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Un2Trek.Trekis.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRowVersionToTrekis : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Trekis",
                type: "rowversion",
                rowVersion: true,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Trekis");
        }
    }
}
