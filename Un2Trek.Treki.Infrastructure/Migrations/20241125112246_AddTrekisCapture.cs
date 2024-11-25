using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Un2Trek.Trekis.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTrekisCapture : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserTrekiCaptures",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TrekiId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ActivityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CaptureDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTrekiCaptures", x => new { x.UserId, x.TrekiId, x.ActivityId });
                    table.ForeignKey(
                        name: "FK_UserTrekiCaptures_ActivityTrekis_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "ActivityTrekis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserTrekiCaptures_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserTrekiCaptures_Trekis_TrekiId",
                        column: x => x.TrekiId,
                        principalTable: "Trekis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserTrekiCaptures_ActivityId",
                table: "UserTrekiCaptures",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTrekiCaptures_TrekiId",
                table: "UserTrekiCaptures",
                column: "TrekiId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserTrekiCaptures");
        }
    }
}
